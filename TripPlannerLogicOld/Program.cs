using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Genetic_V8
{
    public class TripPlannerOld
    {
        public void GenerateRoutes()
        {
            Parameters.rand = new Random();
            PathModifier.getTownsHS();
            Parameters.bestOne = new Individual();
            HashSet<int> usedTowns;
            Individual I = new Individual();
            Population oldPopulation, newPopulation;
            Reproduction breed = new Reproduction();
            int populationSize = 100;
            double chance;
            double totalSumProfit = 0;
            double totalProfit = 0;
            Parameters.solutions.Clear();
            usedTowns = new HashSet<int>();
            for (int z = 0; z < Parameters.daysOfTrip; z++)
            {
                Parameters.bestOne = new Individual();
                Parameters.bestOne.generateFixedIndividual(1, usedTowns);
                oldPopulation = new Population(populationSize);
                for (int i = 0; i < populationSize; i++)
                {
                    int startingTown = Parameters.rand.Next(Parameters.numberOfTowns) + 1;
                    if (usedTowns.Contains(startingTown))
                    {
                        i--;
                        continue;
                    }
                    I = new Individual();
                    I.generateFixedIndividual(startingTown, usedTowns);
                    oldPopulation.Add(I);
                }
                for (int generation = 0; generation < 30; generation++)
                {
                    newPopulation = new Population(populationSize);
                    for (int i = 0; i < populationSize; i++)
                    {
                        chance = Parameters.rand.NextDouble();
                        for (int j = i + 1; j < populationSize; j++)
                        {
                            if (chance < (1 - (i / 2 + j / 2) / populationSize))
                            {
                                I = breed.crossOver(oldPopulation[i], oldPopulation[j], usedTowns);
                                if (!newPopulation.population.Contains(I))
                                    newPopulation.Add(I);
                            }

                        }
                    }
                    oldPopulation = newPopulation;
                    Parameters.Notify(oldPopulation[0]);
                }
                Parameters.totalProfit += Parameters.bestOne.profit;
                Parameters.totalLength += Parameters.bestOne.length;
                Parameters.solutions.Add(Parameters.bestOne);
                for (int k = 0; k < Parameters.bestOne.path.Count; k++)
                {
                    usedTowns.Add(Parameters.bestOne.path[k]);
                }
            }
            Parameters.Notify(null);
            List<int> usedTownsCheck = new List<int>();
            foreach (Individual i in Parameters.solutions)
            {
                for (int x = 0; x < i.path.Count - 1; x++)
                {
                    if (i.path[x] != 0 && usedTownsCheck.Contains(i.path[x]))
                    {
                        Console.WriteLine(i.path[x]);

                    }
                    usedTownsCheck.Add(i.path[x]);
                }
            }
            totalSumProfit += totalProfit;
            StreamWriter sw = new StreamWriter("bestPath.txt");
            foreach (Individual i in Parameters.solutions)
            {
                i.writeIndividualToFile(sw);
            }

            sw.Close();
        }
    }
}
