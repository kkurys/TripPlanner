using System;
using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class RouteGenerator
    {
        public Route GetRoute(int pStartingPoint)
        {
            Route _newRoute = new Route();
            HashSet<int> _availableTowns = new HashSet<int>(Params.AvailablePoints);
            List<int> _newPath = new List<int>();
            Random rand = new Random();
            double _length = 0, _profit = 0;

            _newPath.Add(pStartingPoint);
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

            InsertCapital(ref _newPath);

            _newRoute.Points = _newPath;

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
                return (Params.Profits[pCurrentPoint]) / Params.Distances[pPreviousPoint, pCurrentPoint];
            }
            else
            {
                return -1;
            }
        }
        private void InsertCapital(ref List<int> pPath)
        {
            if (pPath.Contains(0) || pPath.Count == 1) return;
            int _bestInsertionPoint = -1;
            double _minimalDistanceGain = double.MaxValue;
            for (int i = 1; i < pPath.Count - 1; i++)
            {
                if (Params.Distances[i - 1, i] - Params.Distances[pPath[i - 1], 0] - Params.Distances[0, i] < _minimalDistanceGain)
                {
                    _bestInsertionPoint = i;
                    _minimalDistanceGain = Params.Distances[i - 1, i] - Params.Distances[pPath[i - 1], 0] - Params.Distances[0, i];
                }
            }
            pPath.Insert(_bestInsertionPoint, 0);
        }
    }


}
