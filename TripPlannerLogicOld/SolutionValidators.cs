using System.Collections.Generic;

namespace Genetic_V8
{
    public class SolutionValidator
    {
        public bool SolutionHasDuplicates()
        {
            List<int> _usedTowns = new List<int>();
            foreach (Individual solution in Parameters.solutions)
            {
                for (int i = 0; i < solution.path.Count - 1; i++)
                {
                    if (_usedTowns.Contains(solution[i]) && solution[i] != 0)
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
            foreach (Individual solution in Parameters.solutions)
            {
                _currentLength = 0;
                for (int i = 0; i < solution.path.Count - 1; i++)
                {
                    _currentLength += Parameters.distances[solution[i], solution[i + 1]];
                }
                if (_currentLength > Parameters.maxLength)
                {
                    return true;
                }
                _totalLength += _currentLength;
            }
            if (_totalLength != Parameters.totalLength)
            {
                return true;
            }
            return false;
        }
        public bool SolutionTotalProfitIsWrong()
        {
            double _totalProfit = 0;
            foreach (Individual solution in Parameters.solutions)
            {
                for (int i = 0; i < solution.path.Count - 1; i++)
                {
                    _totalProfit += Parameters.profits[solution[i]];
                }
            }
            if (_totalProfit != Parameters.totalProfit)
            {
                return true;
            }
            return false;
        }
    }
}
