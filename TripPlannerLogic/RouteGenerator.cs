using System;
using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class RouteGenerator
    {
        public Route GetRoute(int pStartingPoint)
        {
            Route _newRoute = new Route();
            HashSet<int> _availableTowns = new HashSet<int>(Results.AvailablePoints);
            List<int> _newPath = new List<int>();
            Random rand = new Random();
            double _length = 0, _profit = 0;

            _newPath.Add(pStartingPoint);
            _availableTowns.Remove(pStartingPoint);
            _profit += Params.Profits[pStartingPoint];

            do
            {
                var _bestPoints = GetBestPointsForNextStep(_newPath[_newPath.Count - 1], pStartingPoint, _length, _availableTowns);
                if (_bestPoints.Count == 0)
                {
                    _length += Params.Distances[_newPath[_newPath.Count - 1], pStartingPoint];
                    _newPath.Add(pStartingPoint);
                    break;
                }
                else
                {
                    int t = rand.Next(_bestPoints.Count - 1);
                    _length += Params.Distances[_newPath[_newPath.Count - 1], _bestPoints[t].Point];
                    _newPath.Add(_bestPoints[t].Point);
                    _profit += Params.Profits[_bestPoints[t].Point];
                    _availableTowns.Remove(_bestPoints[t].Point);
                }
            } while (_newPath[_newPath.Count - 1] != _newPath[0]);

            _newRoute.Points = _newPath;
            RouteModificator rm = new RouteModificator(_newRoute);
            rm.InsertCapital();
            //  ModifyPathToBeginWithCapital(ref _newPath);

            RouteCalculator.CalculateRouteProfitAndLength(_newRoute);
            return _newRoute;
        }
        private PointValuePairSortedSet GetBestPointsForNextStep(int pCurrentPoint, int pFinalPoint, double pCurrentLength, HashSet<int> _availableTowns)
        {
            Random rand = new Random();
            PointValuePairSortedSet _bestPoints = new PointValuePairSortedSet(rand.Next(2, 4));

            foreach (int i in _availableTowns)
            {
                double value = CalculateValueBetweenPoints(pCurrentPoint, i, pFinalPoint, pCurrentLength);
                if (value != -1)
                {
                    _bestPoints.Add(new PointValuePair(value, i));
                }
            }


            return _bestPoints;
        }
        private double CalculateValueBetweenPoints(int pPreviousPoint, int pCurrentPoint, int pFinalPoint, double pCurrentLength)
        {
            if (pCurrentLength + Params.Distances[pPreviousPoint, pCurrentPoint] + Params.Distances[pCurrentPoint, pFinalPoint] < Params.MaxLength)
            {
                return (Params.Profits[pCurrentPoint]) * (Params.Profits[pCurrentPoint]) / Params.Distances[pPreviousPoint, pCurrentPoint];
            }
            else
            {
                return -1;
            }
        }

    }


}
