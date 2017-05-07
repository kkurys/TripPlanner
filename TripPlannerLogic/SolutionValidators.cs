using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class SolutionValidator
    {
        public bool SolutionHasDuplicates()
        {
            List<int> _usedTowns = new List<int>();
            foreach (Route solution in Results.Solutions)
            {
                for (int i = 1; i < solution.Points.Count - 1; i++)
                {
                    if (_usedTowns.Contains(solution[i]))
                    {
                        return true;
                    }
                    _usedTowns.Add(solution[i]);
                }
            }
            return false;
        }
        public bool SolutionLengthIsWrong()
        {
            double _totalLength = 0, _currentLength = 0;
            foreach (Route solution in Results.Solutions)
            {
                _currentLength = 0;
                for (int i = 0; i < solution.Points.Count - 1; i++)
                {
                    _currentLength += Params.Distances[solution[i], solution[i + 1]];
                }
                if (_currentLength > Params.MaxLength)
                {
                    return true;
                }
                _totalLength += _currentLength;
            }
            if (_totalLength != Results.TotalLength)
            {
                return true;
            }
            return false;
        }
        public bool SolutionTotalProfitIsWrong()
        {
            double _totalProfit = 0;
            foreach (Route solution in Results.Solutions)
            {
                for (int i = 0; i < solution.Points.Count; i++)
                {
                    _totalProfit += Params.Profits[solution[i]];
                }
            }
            if (_totalProfit != Results.TotalProfit)
            {
                return true;
            }
            return false;
        }
    }
}
