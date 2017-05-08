using System;
using System.Collections.Generic;

namespace Genetic_V8
{
    public static class Parameters
    {
        public static int numberOfTowns;
        public static int maxLength;
        public static int daysOfTrip;
        public static int[,] distances;
        public static double[] profits;
        public static double totalProfit, totalLength;
        public static Random rand;
        public static Individual bestOne;
        public static List<Individual> solutions;
        public static double[,] Coordinates;
        public static void GetParams(string filename)
        {
            FileReader fileIn = new FileReader();
            distances = fileIn.GetDataFromFile(filename, out numberOfTowns, out daysOfTrip, out maxLength, out profits);

            solutions = new List<Individual>();

        }
        public static void PrintDistances()
        {

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Console.Write(distances[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static Notify Notify;
        public static void Init()
        {
            bestOne = new Individual();
            solutions = new List<Individual>();
            totalProfit = 0;
            totalLength = 0;
        }
    }
    public delegate void Notify(Individual route);
}
