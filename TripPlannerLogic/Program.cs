using System;
using System.Collections.Generic;
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
            int populationSize = 40;
            int numberOfGenerations = 80;
            double chance;
            double totalSumProfit = 0;
            double totalProfit = 0;
            bool timeOut = false;
            Parameters.solutions.Clear();
            usedTowns = new HashSet<int>();
            for (int z = 0; z < Parameters.daysOfTrip; z++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                timeOut = false;
                Parameters.bestOne = new Individual();
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
                for (int generation = 0; generation < numberOfGenerations; generation++)
                {
                    if (timeOut) break;
                    newPopulation = new Population(populationSize);
                    for (int i = 0; i < populationSize; i++)
                    {
                        if (timeOut) break;
                        chance = Parameters.rand.NextDouble();
                        for (int j = i + 1; j < populationSize; j++)
                        {
                            if (watch.ElapsedMilliseconds > (Parameters.ExecutionTime - 10) / Parameters.daysOfTrip)
                            {
                                timeOut = true;
                                break;
                            }
                            if (chance < (1 - (i / 2 + j / 2) / populationSize))
                            {
                                if (i < oldPopulation.Count && j < oldPopulation.Count)
                                {
                                    I = breed.crossOver(oldPopulation[i], oldPopulation[j], usedTowns);
                                    if (!newPopulation.population.Contains(I))
                                        newPopulation.Add(I);
                                }

                            }

                        }
                    }
                    oldPopulation = newPopulation;
                    if (oldPopulation.Count > 0)
                    {
                        Parameters.Notify(oldPopulation[0]);
                    }

                }
                Parameters.totalProfit += Parameters.bestOne.profit;
                Parameters.totalLength += Parameters.bestOne.length;
                if (Parameters.bestOne.path.Count == 0)
                {
                    Parameters.bestOne.path.Add(0);
                    Parameters.bestOne.path.Add(0);
                }
                Parameters.bestOne.ModifyPathToBeginWithCapital();
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
            Parameters.SaveToFile();
        }

    }
}
