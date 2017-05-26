using System;
using System.Collections.Generic;
using System.IO;

namespace Genetic_V8
{

    public class Individual
    {
        #region properties
        public List<int> path;
        public double profit { get; set; }
        public int length { get; set; }
        public int Count
        {
            get
            {
                return path.Count;
            }
        }
        public double fitness
        {
            get
            {
                if (length > 1.3 * Parameters.maxLength)
                    return -1;
                else
                    return (profit * profit * profit) * (Parameters.maxLength / length) * (Parameters.maxLength / length) * (Parameters.maxLength / length);
            }
        }
        public int this[int index]
        {
            get
            {
                return path[index];
            }
        }

        #endregion
        #region constructors
        public Individual()
        {
            path = new List<int>();
            profit = 0;
            length = 0;
        }
        public Individual(Individual I)
        {
            path = new List<int>(I.path);
            profit = I.profit;
            length = I.length;
        }
        #endregion
        #region generator
        public void generateFixedIndividual(int startingTown, HashSet<int> usedTowns)
        {
            HashSet<int> availableTowns = new HashSet<int>();
            ContainerForDuos bestTowns = new ContainerForDuos(5);
            PathCalculator calc = new PathCalculator();

            int pathDistance = 0;
            for (int i = 0; i <= Parameters.numberOfTowns; i++)
            {
                if (i == startingTown || usedTowns.Contains(i))
                {
                    continue;
                }

                availableTowns.Add(i);
            }
            List<int> newPath = new List<int>();
            newPath.Add(startingTown);

            profit = 0;
            profit += Parameters.profits[startingTown];
            do
            {
                int k = Parameters.rand.Next(Parameters.To, Parameters.Td);
                bestTowns = new ContainerForDuos(k);
                foreach (int i in availableTowns)
                {
                    double value = calculateValue(newPath[newPath.Count - 1], i, pathDistance, startingTown);
                    if (value != -1)
                    {
                        bestTowns.Add(new Duo(value, i));
                    }
                }
                if (bestTowns.Count == 0)
                {
                    pathDistance += Parameters.distances[newPath[newPath.Count - 1], startingTown];
                    newPath.Add(startingTown);
                    break;
                }
                else
                {
                    int t = Parameters.rand.Next(bestTowns.Count - 1);
                    pathDistance += Parameters.distances[newPath[newPath.Count - 1], bestTowns.town(t)];
                    newPath.Add(bestTowns.town(t));
                    profit += Parameters.profits[bestTowns.town(t)];
                    availableTowns.Remove(bestTowns.town(t));
                }
            }
            while (newPath[newPath.Count - 1] != startingTown);
            length = pathDistance;
            path = newPath;
            insertCapital();
            partialTwoOpt(5);
            evaluatePath();
        }
        public void insertCapital()
        {
            if (path.Contains(0) || path.Count == 1) return;
            int bestInsertionPoint = -1;
            int minimalDistanceGain = int.MaxValue;
            for (int i = 1; i <= path.Count - 2; i++)
            {
                if (Parameters.distances[i - 1, i] - Parameters.distances[path[i - 1], 0] - Parameters.distances[0, i] < minimalDistanceGain)
                {
                    bestInsertionPoint = i;
                    minimalDistanceGain = Parameters.distances[i - 1, i] - Parameters.distances[path[i - 1], 0] - Parameters.distances[0, i];
                }
            }
            path.Insert(bestInsertionPoint, 0);
        }
        double calculateValue(int previousTown, int town, int currentPathLength, int finalTown)
        {
            if (currentPathLength + Parameters.distances[previousTown, town] + Parameters.distances[town, finalTown] < Parameters.maxLength)
            {
                return ((Parameters.profits[town]) * (Parameters.profits[town]) / Parameters.distances[previousTown, town]) * (currentPathLength + Parameters.distances[previousTown, town]) / Parameters.maxLength;
            }
            else
            {
                return -1;
            }

        }
        #endregion
        #region 2opt
        public void partialTwoOpt(int limit)
        {
            int iterations = 0;
            int minChange = 0;
            int minI = -1, minJ = -1;
            do
            {
                minChange = 0;
                for (int i = 1; i < path.Count - 3; i++)
                {
                    for (int j = i + 2; j < path.Count - 2; j++)
                    {
                        int dist = Parameters.distances[path[i], path[j]] + Parameters.distances[path[i + 1], path[j + 1]] - Parameters.distances[path[i], path[i + 1]] - Parameters.distances[path[j], path[j + 1]];
                        if (dist < minChange)
                        {
                            minChange = dist;
                            minI = i;
                            minJ = j;
                        }
                    }
                }
                iterations++;
                if (minChange < 0)
                {
                    path = modifyPath(path, minI, minJ);
                    length += minChange;
                }
            } while (minChange < 0 && iterations < limit);
        }

        public List<int> modifyPath(List<int> path, int lT, int rT)
        {
            List<int> modifiedPath = new List<int>(path);
            int tmp;
            lT++;
            while (lT < rT)
            {
                tmp = modifiedPath[lT];
                modifiedPath[lT] = modifiedPath[rT];
                modifiedPath[rT] = tmp;
                lT++;
                rT--;
            }
            return modifiedPath;
        }
        /*
        public List<int> modifyPath(List<int> path, int lT, int rT)
        {
            List<int> modifiedPath = new List<int>();
            for (int i = 0; i <= lT; i++)
            {
                modifiedPath.Add(path[i]);
            }
            for (int i = rT; i > lT; i--)
            {
                modifiedPath.Add(path[i]);
            }
            for (int i = rT + 1; i < path.Count; i++)
            {
                modifiedPath.Add(path[i]);
            }

            return modifiedPath;
        } */
        #endregion
        public void evaluatePath()
        {
            length = 0;
            profit = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                length += Parameters.distances[path[i], path[i + 1]];
                profit += Parameters.profits[path[i]];
            }
        }
        public void ModifyPathToBeginWithCapital()
        {
            if (path[0] == 0) return;
            int i = 1;
            for (i = 1; i < path.Count; i++)
            {
                path.Add(path[i]);
                if (path[i] == 0)
                {
                    break;
                }
            }
            path.RemoveRange(0, i);
        }

        #region writing
        public void printIndividual()
        {
            Console.WriteLine("Profit: " + profit + " Dist: " + length + " Fitness: " + fitness);
            foreach (int i in path)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }
        public void writeIndividualToFile(StreamWriter sw)
        {
            sw.WriteLine(profit);
            foreach (int i in path)
            {
                sw.Write(i + " ");
            }
            sw.WriteLine();
        }
        #endregion
    }
}
