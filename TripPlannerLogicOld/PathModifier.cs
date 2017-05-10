using System.Collections.Generic;

namespace Genetic_V8
{
    static class PathModifier
    {

        public static HashSet<int> aT;

        #region tryinserting
        public static List<int> tryInserting(List<int> path, HashSet<int> usedTowns)
        {
            List<int> currentPath = new List<int>(path);

            HashSet<int> availableTowns = new HashSet<int>(aT);
            foreach (int k in usedTowns)
            {
                availableTowns.Remove(k);
            }
            PathCalculator PO = new PathCalculator();
            double currentPathLength = PO.calculateDistance(currentPath);
            double currentProfit = 0;
            foreach (int i in currentPath)
            {
                currentProfit += Parameters.profits[i];
                availableTowns.Remove(i);
            }
            double bestPossibleGain = 0;
            int bestPossibleGainIndex = -1;
            int bestPossibleGainTown = -1;
            double bestPossibleGainLengthInc = 0;
            int iterations = 0;
            do
            {
                iterations++;
                bestPossibleGain = 0;
                bestPossibleGainIndex = 0;
                bestPossibleGainTown = -1;
                foreach (int i in availableTowns)
                {
                    for (int x = 0; x < currentPath.Count - 1; x++)
                    {
                        double dist = Parameters.distances[currentPath[x], i] + Parameters.distances[i, currentPath[x + 1]] - Parameters.distances[currentPath[x], currentPath[x + 1]];
                        if (currentPathLength + dist <= Parameters.maxLength)
                        {

                            if (Parameters.profits[i] * Parameters.profits[i] * Parameters.profits[i] / (currentPathLength + dist) * Parameters.maxLength / (currentPathLength + dist) > bestPossibleGain)
                            {

                                bestPossibleGainLengthInc = Parameters.distances[currentPath[x], i] + Parameters.distances[i, currentPath[x + 1]] - Parameters.distances[currentPath[x], currentPath[x + 1]];
                                bestPossibleGain = Parameters.profits[i] * Parameters.profits[i] * Parameters.profits[i] / (currentPathLength + dist) * Parameters.maxLength / (currentPathLength + dist);
                                bestPossibleGainIndex = x;
                                bestPossibleGainTown = i;
                            }
                        }
                    }
                }
                if (bestPossibleGainTown != -1)
                {
                    currentPath.Insert(bestPossibleGainIndex + 1, bestPossibleGainTown);
                    currentProfit += bestPossibleGain;
                    availableTowns.Remove(bestPossibleGainTown);
                    currentPathLength = PO.calculateDistance(currentPath);
                }

            } while (bestPossibleGain != 0 && currentPathLength <= Parameters.maxLength && iterations < Parameters.rand.Next(1));

            return currentPath;
        }

        internal static void tryInverting(Individual child)
        {
            int startIdx = Parameters.rand.Next(2, child.Count - 2);
            int side = Parameters.rand.Next();
            if (side % 2 == 0)
            {
                child.path = child.modifyPath(child.path, startIdx, child.Count - 2);
            }
            else
            {
                child.path = child.modifyPath(child.path, 1, startIdx);
            }
        }
        #endregion
        #region tryswapping
        public static List<int> trySwapping(List<int> path, HashSet<int> usedTowns)
        {
            List<int> currentPath = new List<int>(path);
            HashSet<int> availableTowns = new HashSet<int>(aT);
            foreach (int k in usedTowns)
            {
                availableTowns.Remove(k);
            }
            PathCalculator PO = new PathCalculator();
            List<int> added = new List<int>();
            List<int> removed = new List<int>();
            double currentPathLength = PO.calculateDistance(currentPath);
            double currentProfit = 0;

            foreach (int i in currentPath)
            {
                currentProfit += Parameters.profits[i];
                availableTowns.Remove(i);
            }
            for (int i = 2; i < currentPath.Count - 1; i++)
            {
                if (currentPath[i] == 1) continue;
                foreach (int k in availableTowns)
                {
                    if (Parameters.profits[k] >= Parameters.profits[path[i]])
                    {
                        if (Parameters.profits[k] == Parameters.profits[path[i]])
                        {
                            if (Parameters.distances[currentPath[i - 1], currentPath[i]] + Parameters.distances[currentPath[i], currentPath[i + 1]] > Parameters.distances[currentPath[i - 1], k] + Parameters.distances[k, currentPath[i + 1]])
                            {
                                added.Add(currentPath[i]);
                                removed.Add(k);
                                currentPath[i] = k;
                            }
                        }
                        else
                        {
                            if (currentPathLength - Parameters.distances[currentPath[i - 1], currentPath[i]] - Parameters.distances[currentPath[i], currentPath[i + 1]] + Parameters.distances[currentPath[i - 1], k] + Parameters.distances[k, currentPath[i + 1]] <= Parameters.maxLength)
                            {
                                added.Add(path[i]);
                                removed.Add(k);
                                currentPath[i] = k;
                            }
                        }
                    }
                }
                foreach (int x in added)
                {
                    availableTowns.Add(x);
                }
                foreach (int x in removed)
                {
                    availableTowns.Remove(x);
                }
                added.Clear();
                removed.Clear();

            }
            return currentPath;
        }
        #endregion
        #region tryexch
        public static void tryExchanging(Individual I, HashSet<int> usedTowns)
        {
            HashSet<int> availableTowns = new HashSet<int>(aT);
            foreach (int k in usedTowns)
            {
                availableTowns.Remove(k);
            }
            foreach (int i in I.path)
            {
                availableTowns.Remove(i);
            }
            double bestProfitGained = -1;
            int indexOfTownToSwap = -1, townToSwap = -1;

            for (int i = 1; i < I.Count - 1; i++)
            {
                if (I[i] == 1) continue;
                foreach (int z in availableTowns)
                {
                    double currentProfitGained = Parameters.profits[z] - Parameters.profits[I[i]];
                    if (currentProfitGained > bestProfitGained && I.length - Parameters.distances[I[i], I[i - 1]] - Parameters.distances[I[i], I[i + 1]] + Parameters.distances[I[i - 1], z] + Parameters.distances[z, I[i + 1]] <= Parameters.maxLength)
                    {
                        bestProfitGained = currentProfitGained;
                        indexOfTownToSwap = i;
                        townToSwap = z;
                    }
                }
            }
            if (indexOfTownToSwap != -1)
                I.path[indexOfTownToSwap] = townToSwap;
        }
        #endregion
        #region trymoving
        public static List<int> tryMoving(List<int> path, HashSet<int> usedTowns)
        {
            List<int> bestPath = new List<int>(path);
            PathCalculator calc = new PathCalculator();
            double bestPathProfit;
            double newProfit = 0;
            double bestPathDist;
            evaluatePath(bestPath, out bestPathDist, out bestPathProfit);
            for (int i = 1; i < bestPath.Count - 2; i++)
            {
                for (int j = 1; j < bestPath.Count - 3; j++)
                {
                    if (i == j) continue;
                    double newDist = bestPathDist;
                    newProfit = 0;

                    if (j == i + 1)
                    {
                        newDist = newDist - Parameters.distances[bestPath[i - 1], bestPath[i]] + Parameters.distances[bestPath[i - 1], bestPath[j]] + Parameters.distances[bestPath[i], bestPath[j + 1]] - Parameters.distances[bestPath[j], bestPath[j + 1]];
                    }
                    else if (i == j + 1)
                    {
                        newDist += -Parameters.distances[bestPath[j - 1], bestPath[j]] + Parameters.distances[bestPath[j - 1], bestPath[j + 1]] + Parameters.distances[bestPath[j], bestPath[j + 1]];
                    }
                    else if (i > j)
                    {
                        newDist = newDist - Parameters.distances[bestPath[i - 1], bestPath[i]] - Parameters.distances[bestPath[i], bestPath[i + 1]]
                                   - Parameters.distances[bestPath[j], bestPath[j + 1]]
                                   + Parameters.distances[bestPath[i - 1], bestPath[i + 1]] + Parameters.distances[bestPath[j], bestPath[i]] + Parameters.distances[bestPath[i], bestPath[j + 1]];
                    }
                    else if (j > i)
                    {
                        newDist += -Parameters.distances[bestPath[j - 1], bestPath[j]] - Parameters.distances[bestPath[j], bestPath[j + 1]]
                                   - Parameters.distances[bestPath[i - 1], bestPath[i]]
                                   + Parameters.distances[bestPath[j - 1], bestPath[j + 1]] + Parameters.distances[bestPath[i - 1], bestPath[j]] + Parameters.distances[bestPath[j], bestPath[i]];
                    }
                    if (newDist >= Parameters.maxLength) continue;
                    List<int> newPath = new List<int>(bestPath);

                    int tmp = newPath[i];
                    newPath.Remove(newPath[i]);
                    newPath.Insert(j, tmp);
                    newPath = tryInserting(newPath, usedTowns);
                    evaluatePath(newPath, out newDist, out newProfit);
                    if (newProfit > bestPathProfit)
                    {
                        bestPath = newPath;
                        bestPathProfit = newProfit;
                        bestPathDist = newDist;
                    }
                }
            }

            return bestPath;
        }
        #endregion
        #region tryRemoving
        static public void tryRemoveChange(Individual I, HashSet<int> usedTowns)
        {
            HashSet<int> availableTowns = new HashSet<int>(aT);
            ContainerForDuos townsToRemove = new ContainerForDuos(Parameters.rand.Next(1, I.Count / 2));
            foreach (int k in usedTowns)
            {
                availableTowns.Remove(k);
            }
            for (int i = 1; i < I.Count - 1; i++)
            {
                if (I[i] == 1) continue;
                double value = (Parameters.distances[I[i - 1], I[i]] + Parameters.distances[I[i], I[i + 1]]) / Parameters.profits[I[i]] * Parameters.profits[I[i]] * Parameters.profits[I[i]];
                if (townsToRemove.Count < townsToRemove.size)
                {
                    townsToRemove.Add(new Duo(value, I[i]));
                }
                else
                {
                    if (value > townsToRemove[townsToRemove.size - 1].value)
                    {
                        townsToRemove.Add(new Duo(value, I[i]));
                    }
                }
            }
            foreach (Duo D in townsToRemove)
            {
                I.path.Remove(D.index);
            }
            I.evaluatePath();
            I.path = tryInserting(I.path, usedTowns);
            I.evaluatePath();
        }
        #endregion
        public static void tryMutate(Individual I, HashSet<int> usedTowns)
        {
            List<int> availableTowns = new List<int>(aT);
            foreach (int i in I.path)
            {
                availableTowns.Remove(i);
            }
            foreach (int k in usedTowns)
            {
                availableTowns.Remove(k);
            }
            int maxValueTown = 0;
            double maxValueProfit = 0;

            for (int i = 0; i < availableTowns.Count; i++)
            {
                if (Parameters.profits[availableTowns[i]] > maxValueProfit)
                {
                    maxValueTown = i;
                    maxValueProfit = Parameters.profits[availableTowns[i]];
                }
            }

            int z = Parameters.rand.Next(I.Count - 2) + 2;

            //   I.path[z] = availableTowns[Parameters.rand.Next(availableTowns.Count - 1)];
            I.path[z] = availableTowns[maxValueTown];

        }
        public static void evaluatePath(List<int> path, out double length, out double profit)
        {
            length = 0;
            profit = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                length += Parameters.distances[path[i], path[i + 1]];
                profit += Parameters.profits[path[i]];
            }
        }


        public static void getTownsHS()
        {
            aT = new HashSet<int>();
            for (int i = 2; i <= Parameters.numberOfTowns; i++)
            {
                aT.Add(i);
            }
        }

    }
}
