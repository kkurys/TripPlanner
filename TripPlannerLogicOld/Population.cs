﻿using System.Collections;
using System.IO;
using System.Linq;

namespace Genetic_V8
{
    class Population : IEnumerable
    {
        public ContainerForInds population;
        public double averagePopulationFitness = 0;
        public int Count
        {
            get
            {
                return population.Count();
            }
        }
        public Individual this[int index]
        {
            get
            {
                return population[index];
            }
        }
        public Population(int s)
        {
            population = new ContainerForInds(s);
        }
        public void Add(Individual p)
        {
            population.Add(p);
            averagePopulationFitness = (averagePopulationFitness + p.fitness) / Count;
            if (p.profit >= Parameters.bestOne.profit && p.length <= Parameters.maxLength && p.path.Contains(0))
            {
                Parameters.bestOne = p;
            }
        }
        public void printPopulation()
        {
            int i = 0;
            foreach (Individual I in population)
            {
                I.printIndividual();
                i++;
                if (i == 10) break;
            }
        }
        public void printFittest()
        {
            population.container[0].printIndividual();
            StreamWriter sw = new StreamWriter("bestPath.txt");
            population.container[0].writeIndividualToFile(sw);
            sw.Close();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)population).GetEnumerator();
        }
    }
}
