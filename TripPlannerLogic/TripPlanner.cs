using System;

namespace TripPlannerLogic
{
    public class TripPlanner
    {
        private RouteGenerator _routeGenerator;
        private RouteSortedSet _oldPopulation, _newPopulation;
        private RouteCrossing _routeCrossing;
        private RouteOptimizator _routeOptimizator;
        private RouteModificator _routeModificator;
        private Random _rand;
        private int _populationSize = 100;
        private int _numberOfGenerations = 50;

        public TripPlanner()
        {
            _rand = new Random();
            _routeGenerator = new RouteGenerator();
            _routeCrossing = new RouteCrossing();
            _routeOptimizator = new RouteOptimizator();
            _routeModificator = new RouteModificator();
        }
        public void GenerateRoutes()
        {
            for (int day = 0; day < Params.DaysOfTrip; day++)
            {
                GenerateInitialPopulation();
                for (int generation = 0; generation < _numberOfGenerations; generation++)
                {
                    _oldPopulation = CreateNewPopulation();
                }

                for (int z = 0; z < Results.CurrentBestOne.Count; z++)
                {
                    Params.AvailablePoints.Remove(Results.CurrentBestOne[z]);
                }
                Results.Solutions.Add(Results.CurrentBestOne);
                Results.TotalLength += Results.CurrentBestOne.Length;
                Results.TotalProfit += Results.CurrentBestOne.Profit;
            }
        }
        private void GenerateInitialPopulation()
        {
            _oldPopulation = new RouteSortedSet(_populationSize);
            for (int i = 0; i < _populationSize; i++)
            {
                int startingPoint = _rand.Next(Params.NumberOfPoints + 1);
                _oldPopulation.Add(_routeGenerator.GetRoute(startingPoint));
            }
        }
        private RouteSortedSet CreateNewPopulation()
        {
            double _chance;
            _newPopulation = new RouteSortedSet(_populationSize);
            for (int i = 0; i < _populationSize; i++)
            {
                _chance = _rand.NextDouble();
                for (int j = i + 1; j < _populationSize; j++)
                {
                    if (_chance < (1 - (i / 2 + j / 2) / 100))
                    {
                        Route newRoute = _routeCrossing.Cross(_oldPopulation[i], _oldPopulation[j]);
                        if (!_newPopulation.Contains(newRoute))
                        {
                            _newPopulation.Add(newRoute);
                        }

                    }
                }
            }
            return _newPopulation;
        }
    }
}
