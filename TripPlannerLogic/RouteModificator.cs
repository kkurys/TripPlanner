using System;
using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class RouteModificator
    {
        private Random _rand;
        private Route _route;
        public List<int> AvailablePoints { get; set; }
        public RouteModificator(Route route)
        {
            _rand = new Random();
            _route = route;
            AvailablePoints = new List<int>(Results.AvailablePoints);
            foreach (int p in route.Points)
            {
                AvailablePoints.Remove(p);
            }
        }
        public void InsertBestPointAtBestIndex()
        {
            if (AvailablePoints.Count == 0) return;
            int pointToInsert = 0;
            double pointToInsertProfit = 0;
            for (int i = 0; i < AvailablePoints.Count; i++)
            {
                if (Params.Profits[AvailablePoints[i]] > pointToInsertProfit)
                {
                    pointToInsertProfit = Params.Profits[AvailablePoints[i]];
                    pointToInsert = AvailablePoints[i];
                }
            }
            double maxValue = 0, value = 0, distChange = 0;
            int bestPos = 0;
            for (int i = 0; i < _route.Count - 2; i++)
            {
                distChange = (Params.Distances[_route[i], pointToInsert] + Params.Distances[pointToInsert, _route[i + 1]] - Params.Distances[_route[i], _route[i + 1]]);
                value = pointToInsertProfit * pointToInsertProfit / distChange;
                if (value > maxValue && _route.Length + distChange < 1.3 * Params.MaxLength)
                {
                    maxValue = value;
                    bestPos = i + 1;
                }
            }
            _route.Points.Insert(bestPos, pointToInsert);
            AvailablePoints.Remove(pointToInsert);
        }
        public void RandomlySwapTwoPoints()
        {
            int point1 = _rand.Next() % (_route.Count - 1) + 1;
            int point2 = _rand.Next() % (_route.Count - 1) + 1;
            int tmp = _route[point1];
            _route[point1] = _route[point2];
            _route[point2] = tmp;
        }
        public void RandomlySwapPointForBest()
        {
            if (AvailablePoints.Count == 0) return;
            int pointToInsert = 0;
            double pointToInsertProfit = 0;
            int insertionPoint = _rand.Next() % (_route.Count - 2) + 1;
            for (int i = 0; i < AvailablePoints.Count; i++)
            {
                if (Params.Profits[AvailablePoints[i]] > pointToInsertProfit)
                {
                    pointToInsertProfit = Params.Profits[AvailablePoints[i]];
                    pointToInsert = AvailablePoints[i];
                }
            }
            AvailablePoints.Add(_route[insertionPoint]);
            _route[insertionPoint] = pointToInsert;
            AvailablePoints.Remove(pointToInsert);
        }
        public void RemoveWeakestPoint()
        {
            if (_route.Count < 3) return;
            double maxValue = 0;
            int deletionPoint = 0;
            for (int i = 1; i < _route.Count - 1; i++)
            {
                double value = (Params.Distances[_route[i - 1], _route[i]] + Params.Distances[_route[i], _route[i + 1]] - Params.Distances[_route[i - 1], _route[i + 1]]) / Params.Profits[_route[i]];
                if (value > maxValue)
                {
                    deletionPoint = i;
                    maxValue = value;
                }
            }

            AvailablePoints.Add(_route.Points[deletionPoint]);
            _route.Points.RemoveAt(deletionPoint);
        }
        public void Move()
        {
            List<int> bestPath = new List<int>(_route.Points);
            double bestPathProfit = _route.Profit;
            double bestPathDist = _route.Length;

            for (int i = 1; i < bestPath.Count - 2; i++)
            {
                for (int j = 1; j < bestPath.Count - 3; j++)
                {
                    if (i == j) continue;
                    double newDist = bestPathDist;

                    if (j == i + 1)
                    {
                        newDist = newDist - Params.Distances[bestPath[i - 1], bestPath[i]] + Params.Distances[bestPath[i - 1], bestPath[j]] + Params.Distances[bestPath[i], bestPath[j + 1]] - Params.Distances[bestPath[j], bestPath[j + 1]];
                    }
                    else if (i == j + 1)
                    {
                        newDist += -Params.Distances[bestPath[j - 1], bestPath[j]] + Params.Distances[bestPath[j - 1], bestPath[j + 1]] + Params.Distances[bestPath[j], bestPath[j + 1]];
                    }
                    else if (i > j)
                    {
                        newDist = newDist - Params.Distances[bestPath[i - 1], bestPath[i]] - Params.Distances[bestPath[i], bestPath[i + 1]]
                                   - Params.Distances[bestPath[j], bestPath[j + 1]]
                                   + Params.Distances[bestPath[i - 1], bestPath[i + 1]] + Params.Distances[bestPath[j], bestPath[i]] + Params.Distances[bestPath[i], bestPath[j + 1]];
                    }
                    else if (j > i)
                    {
                        newDist += -Params.Distances[bestPath[j - 1], bestPath[j]] - Params.Distances[bestPath[j], bestPath[j + 1]]
                                   - Params.Distances[bestPath[i - 1], bestPath[i]]
                                   + Params.Distances[bestPath[j - 1], bestPath[j + 1]] + Params.Distances[bestPath[i - 1], bestPath[j]] + Params.Distances[bestPath[j], bestPath[i]];
                    }
                    if (newDist >= Params.MaxLength) continue;
                    List<int> newPath = new List<int>(bestPath);

                    int tmp = newPath[i];
                    newPath.Remove(newPath[i]);
                    newPath.Insert(j, tmp);
                    Route route = new Route();
                    route.Points = newPath;

                    RouteCalculator.CalculateRouteProfitAndLength(route);
                    if (route.Profit > bestPathProfit)
                    {
                        _route.Points = newPath;
                        _route.Profit = route.Profit;
                        _route.Length = route.Length;
                    }
                }
            }

        }
        public void Swap()
        {
            List<int> added = new List<int>();
            List<int> removed = new List<int>();
            double currentPathLength = _route.Length;
            double currentProfit = _route.Profit;

            for (int i = 2; i < _route.Count - 1; i++)
            {
                if (_route[i] == 0) continue;
                foreach (int k in AvailablePoints)
                {
                    if (Params.Profits[k] >= Params.Profits[_route[i]])
                    {
                        if (Params.Profits[k] == Params.Profits[_route[i]])
                        {
                            if (Params.Distances[_route[i - 1], _route[i]] + Params.Distances[_route[i], _route[i + 1]] > Params.Distances[_route[i - 1], k] + Params.Distances[k, _route[i + 1]])
                            {
                                added.Add(_route[i]);
                                removed.Add(k);
                                _route[i] = k;
                            }
                        }
                        else
                        {
                            if (currentPathLength - Params.Distances[_route[i - 1], _route[i]] - Params.Distances[_route[i], _route[i + 1]] + Params.Distances[_route[i - 1], k] + Params.Distances[k, _route[i + 1]] <= Params.MaxLength)
                            {
                                added.Add(_route[i]);
                                removed.Add(k);
                                _route[i] = k;
                            }
                        }
                    }
                }
                foreach (int x in added)
                {
                    AvailablePoints.Add(x);
                }
                foreach (int x in removed)
                {
                    AvailablePoints.Remove(x);
                }
                added.Clear();
                removed.Clear();

            }
        }
        public void RemoveAndChange()
        {
            PointValuePairSortedSet townsToRemove = new PointValuePairSortedSet(_rand.Next(1, _route.Count / 2));

            for (int i = 1; i < _route.Count - 1; i++)
            {
                if (_route[i] == 0) continue;
                double value = (Params.Distances[_route[i - 1], _route[i]] + Params.Distances[_route[i], _route[i + 1]]) / Params.Profits[_route[i]] * Params.Profits[_route[i]] * Params.Profits[_route[i]];
                if (townsToRemove.Count < townsToRemove.Size)
                {
                    townsToRemove.Add(new PointValuePair(value, _route[i]));
                }
                else
                {
                    if (value > townsToRemove[townsToRemove.Size - 1].Value)
                    {
                        townsToRemove.Add(new PointValuePair(value, _route[i]));
                    }
                }
            }
            foreach (PointValuePair D in townsToRemove)
            {
                _route.Points.Remove(D.Point);
            }
            RouteCalculator.CalculateRouteProfitAndLength(_route);
            MassInsert();
        }
        public void MassInsert()
        {
            double currentPathLength = _route.Length;
            double currentProfit = _route.Profit;

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
                foreach (int i in AvailablePoints)
                {
                    for (int x = 0; x < _route.Count - 1; x++)
                    {
                        if (currentPathLength + Params.Distances[_route[x], i] + Params.Distances[i, _route[x + 1]] - Params.Distances[_route[x], _route[x + 1]] <= Params.MaxLength)
                        {

                            if (Params.Profits[i] > bestPossibleGain)
                            {

                                bestPossibleGainLengthInc = Params.Distances[_route[x], i] + Params.Distances[i, _route[x + 1]] - Params.Distances[_route[x], _route[x + 1]];
                                bestPossibleGain = Params.Profits[i];
                                bestPossibleGainIndex = x;
                                bestPossibleGainTown = i;
                            }
                        }
                    }
                }
                if (bestPossibleGainTown != -1)
                {
                    _route.Points.Insert(bestPossibleGainIndex + 1, bestPossibleGainTown);
                    AvailablePoints.Remove(bestPossibleGainTown);
                    RouteCalculator.CalculateRouteProfitAndLength(_route);
                }

            } while (bestPossibleGain != 0 && currentPathLength <= Params.MaxLength && iterations < 1);

        }
        public void InsertCapital()
        {
            if (_route.Contains(0) || _route.Count == 1) return;
            int _bestInsertionPoint = -1;
            double _minimalDistanceGain = double.MaxValue;
            for (int i = 1; i < _route.Count - 2; i++)
            {
                if (Params.Distances[i - 1, i] - Params.Distances[_route[i - 1], 0] - Params.Distances[0, i] < _minimalDistanceGain)
                {
                    _bestInsertionPoint = i;
                    _minimalDistanceGain = Params.Distances[i - 1, i] - Params.Distances[_route[i - 1], 0] - Params.Distances[0, i];
                }
            }
            _route.Points.Insert(_bestInsertionPoint, 0);
        }
        public void ModifyPathToBeginWithCapital()
        {
            if (_route[0] == 0) return;
            int i = 1;
            for (i = 1; i < _route.Count; i++)
            {
                _route.Points.Add(_route[i]);
                if (_route[i] == 0)
                {
                    break;
                }
            }
            _route.Points.RemoveRange(0, i);
        }
    }
}
