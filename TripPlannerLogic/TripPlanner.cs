using System;
using System.Collections.Generic;

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
        private int _populationSize = 200;
        private int _numberOfGenerations = 10;

        public TripPlanner()
        {
            _rand = new Random();
            _routeGenerator = new RouteGenerator();
            _routeCrossing = new RouteCrossing();
            _routeOptimizator = new RouteOptimizator();
        }
        public void GenerateRoutes()
        {
            Results.Solutions = new List<Route>();
            for (int day = 0; day < Params.DaysOfTrip; day++)
            {
                Results.CurrentBestOne = new Route();
                GenerateInitialPopulation();
                for (int generation = 0; generation < _numberOfGenerations; generation++)
                {
                    _oldPopulation = CreateNewPopulation();
                    Results.Notify(_oldPopulation[0]);
                }

                for (int z = 0; z < Results.CurrentBestOne.Count; z++)
                {
                    Results.AvailablePoints.Remove(Results.CurrentBestOne[z]);
                }
                if (_routeModificator == null)
                {
                    _routeModificator = new RouteModificator(Results.CurrentBestOne);
                }
                _routeModificator.ModifyPathToBeginWithCapital();

                Results.TotalLength += Results.CurrentBestOne.Length;
                Results.TotalProfit += Results.CurrentBestOne.Profit;
                Results.Solutions.Add(Results.CurrentBestOne);

                _routeModificator = null;
            }
            Results.Notify(null);
        }
        private void GenerateInitialPopulation()
        {
            _oldPopulation = new RouteSortedSet(_populationSize);
            for (int i = 0; i < _populationSize; i++)
            {
                int startingPoint = _rand.Next(Params.NumberOfPoints + 1);
                if (!Results.AvailablePoints.Contains(startingPoint))
                {
                    i--;
                    continue;
                }
                Route _newRoute = _routeGenerator.GetRoute(startingPoint);
                int numberOfIteratios = _rand.Next() % 3 + 1;
                _routeOptimizator.TwoOptimal(_newRoute, numberOfIteratios);
                _oldPopulation.Add(_newRoute);
            }
        }
        private RouteSortedSet CreateNewPopulation()
        {
            double _chance;
            _newPopulation = new RouteSortedSet(_populationSize);
            for (int i = 0; i < _populationSize - 1; i++)
            {
                _chance = _rand.NextDouble();
                for (int j = i + 1; j < _populationSize; j++)
                {
                    if (_chance < (1 - (i / 2 + j / 2) / 100))
                    {
                        Route newRoute = _routeCrossing.Cross(_oldPopulation[i], _oldPopulation[j]);
                        double oldDist = newRoute.Length;

                        _routeOptimizator.TwoOptimal(newRoute, 2);
                        Mutate(_chance, newRoute);
                        newRoute = CompareChildAndParents(newRoute, i, j);
                        if (_routeModificator == null)
                        {
                            _routeModificator = new RouteModificator(newRoute);
                        }

                        _routeModificator.InsertCapital();
                        RouteCalculator.CalculateRouteProfitAndLength(newRoute);


                        //    if (newRoute.Length > oldDist) throw new Exception("2opt did something wrong!");

                        if (!_newPopulation.Contains(newRoute))
                        {
                            _newPopulation.Add(newRoute);
                        }
                        _routeModificator = null;
                    }

                }
            }
            return _newPopulation;
        }
        public void Mutate(double _chance, Route _newRoute)
        {
            if (_chance > 0.9965)
            {
                if (_routeModificator == null)
                {
                    _routeModificator = new RouteModificator(_newRoute);
                }
                _routeModificator.Move();
            }
            else if (_chance > 0.96)
            {
                if (_routeModificator == null)
                {
                    _routeModificator = new RouteModificator(_newRoute);
                }
                _routeModificator.RemoveAndChange();
            }

            else if (_chance < 0.4 && _chance > 0.05)
            {
                if (_routeModificator == null)
                {
                    _routeModificator = new RouteModificator(_newRoute);
                }
                _routeModificator.RandomlySwapPointForBest();
            }
            else if (_chance > 0.6 && _chance < 0.63)
            {
                if (_routeModificator == null)
                {
                    _routeModificator = new RouteModificator(_newRoute);
                }
                _routeModificator.Swap();
            }
            /*       if (_chance > 0.8)
                   {
                       if (_routeModificator == null)
                       {
                           _routeModificator = new RouteModificator(_newRoute);
                       }
                       int numberOfPointsToRemove = _rand.Next() % 6 + 1;
                       for (int z = 0; z < numberOfPointsToRemove; z++)
                       {
                           _routeModificator.RemoveWeakestPoint();
                       }

                   } */
            /*
            if (_chance > 0.6)
            {
                if (_routeModificator == null)
                {
                    _routeModificator = new RouteModificator(_newRoute);
                }
                if (_routeModificator.AvailablePoints.Count > 0)
                {
                    int numberOfPointsToAdd = _rand.Next() % 8 + 1;
                    for (int z = 0; z < numberOfPointsToAdd; z++)
                    {
                        _routeModificator.InsertBestPointAtBestIndex();
                    }
                }

            }*/
            else if (_chance > 0.7 && _chance < 0.73)
            {
                int numberOfIteratios = _rand.Next() % 3 + 1;
                _routeOptimizator.TwoOptimal(_newRoute, numberOfIteratios);
            }
        }
        private Route CompareChildAndParents(Route newRoute, int i, int j)
        {
            int similarityToParent1 = 0, similarityToParent2 = 0;
            double pSimilarityToParent1 = 0, pSimilarityToParent2 = 0;
            for (int k = 0; k < newRoute.Count; k++)
            {
                if (k < _oldPopulation[i].Count)
                {
                    if (newRoute[k] == _oldPopulation[i][k])
                    {
                        similarityToParent1++;
                    }
                }
                if (k < _oldPopulation[j].Count)
                {
                    if (newRoute[k] == _oldPopulation[j][k])
                    {
                        similarityToParent2++;
                    }
                }
                if (k >= _oldPopulation[j].Count && k >= _oldPopulation[i].Count) break;
            }
            pSimilarityToParent1 = similarityToParent1 / (newRoute.Count);
            pSimilarityToParent2 = similarityToParent2 / (newRoute.Count);
            if (pSimilarityToParent1 >= pSimilarityToParent2)
            {
                if (_oldPopulation[i].Fitness > newRoute.Fitness)
                {
                    return _oldPopulation[i];
                }

            }
            if (pSimilarityToParent2 >= pSimilarityToParent1)
            {
                if (_oldPopulation[j].Fitness > newRoute.Fitness)
                {
                    return _oldPopulation[j];
                }
            }
            return newRoute;
        }
    }
}
