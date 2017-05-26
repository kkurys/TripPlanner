using System.Collections.Generic;

namespace Genetic_V8
{
    public class Reproduction
    {
        public Individual crossOver(Individual parent1, Individual parent2, HashSet<int> usedTowns)
        {
            Individual child = new Individual();
            int point = parent1.Count > parent2.Count ? Parameters.rand.Next(parent2.Count - 2) + 2 : Parameters.rand.Next(parent1.Count - 2) + 2;
            child.path.Add(parent1.path[0]);
            child.length = 0;
            child.profit = 0;
            bool[] wasUsed = new bool[Parameters.numberOfTowns + 1];
            wasUsed[parent1[0]] = true;
            for (int i = 1; i < point; i++)
            {
                child.path.Add(parent1.path[i]);
                child.profit += Parameters.profits[parent1.path[i]];
                child.length += Parameters.distances[child.path[i - 1], child.path[i]];
                wasUsed[parent1.path[i]] = true;
            }
            // child.path.Contains(parent2.path[i])
            for (int i = point; i < parent2.path.Count - 1; i++)
            {
                if (!wasUsed[parent2[i]] && child.length + Parameters.distances[child.path[child.Count - 1], parent2.path[i]] + Parameters.distances[child.path[0], parent2.path[i]] <= 1.03 * Parameters.maxLength)
                {
                    child.path.Add(parent2.path[i]);
                    child.profit += Parameters.profits[parent2.path[i]];
                    child.length += Parameters.distances[child.path[child.Count - 1], child.path[child.Count - 2]];
                }
            }
            child.path.Add(child.path[0]);
            child.length += Parameters.distances[child.path[child.Count - 2], child.path[child.Count - 1]];
            child.insertCapital();

            double chance = Parameters.rand.NextDouble();
            if (chance > Parameters.Ptrc)
            {
                PathModifier.tryRemoveChange(child, usedTowns);
            }
            else if (chance < Parameters.Pmt)
            {
                PathModifier.tryMutate(child, usedTowns);
            }
            else if (chance > Parameters.Pmv)
            {
                child.path = PathModifier.tryMoving(child.path, usedTowns);
            }
            else if (chance > 0.7 && chance < 0.72)
            {
                child.partialTwoOpt(Parameters.rand.Next(1, 5));
            }
            else if (chance > 0.6 && chance < 0.615)
            {
                child.path = PathModifier.trySwapping(child.path, usedTowns);
            }
            else if (chance > 0.4)
            {
                PathModifier.tryInverting(child);
            }
            child.evaluatePath();
            int similarityToParent1 = 0, similarityToParent2 = 0;
            double pSimilarityToParent1 = 0, pSimilarityToParent2 = 0;
            for (int i = 0; i < child.Count; i++)
            {
                if (i < parent1.Count)
                {
                    if (child.path[i] == parent1.path[i])
                    {
                        similarityToParent1++;
                    }
                }
                if (i < parent2.Count)
                {
                    if (child.path[i] == parent2.path[i])
                    {
                        similarityToParent2++;
                    }
                }
                if (i >= parent2.Count && i >= parent1.Count) break;
            }
            pSimilarityToParent1 = similarityToParent1 / (child.Count);
            pSimilarityToParent2 = similarityToParent2 / (child.Count);
            if (pSimilarityToParent1 >= pSimilarityToParent2)
            {
                if (parent1.fitness > child.fitness)
                {
                    return parent1;
                }

            }
            if (pSimilarityToParent2 >= pSimilarityToParent1)
            {
                if (parent2.fitness > child.fitness)
                {
                    return parent2;
                }
            }

            return child;
        }
    }
}
