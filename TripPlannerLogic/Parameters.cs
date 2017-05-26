using System;
using System.Collections.Generic;
using System.IO;

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
        public static double DrawModifier;
        public static void GetParams(string filename)
        {
            FileReader fileIn = new FileReader();
            distances = fileIn.GetDataFromFile(filename, out numberOfTowns, out daysOfTrip, out maxLength, out profits);

            solutions = new List<Individual>();

        }

        public static Notify Notify;
        public static void Init()
        {
            bestOne = new Individual();
            solutions = new List<Individual>();
            totalProfit = 0;
            totalLength = 0;
        }
        public static int To = 2, Td = 4;
        public static double Ptrc = 0.97, Pmv = 0.85, Pmt = 0.2;
        public static void SaveToFile()
        {
            using (StreamWriter sw = new StreamWriter("solutions.txt"))
            {
                foreach (Individual i in solutions)
                {
                    sw.WriteLine(i.profit + " " + i.length);
                    foreach (int x in i.path)
                    {
                        sw.Write(x + " ");
                    }
                }
            }
        }
    }
    public delegate void Notify(Individual route);


}
