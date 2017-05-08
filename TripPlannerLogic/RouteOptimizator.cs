using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class RouteOptimizator
    {
        public void TwoOptimal(Route path, int limit)
        {
            int iterations = 0;
            RouteCalculator.CalculateRouteProfitAndLength(path);
            double minChange = 0;
            int minI = -1, minJ = -1;
            do
            {
                minChange = 0;
                for (int i = 1; i < path.Count - 3; i++)
                {
                    for (int j = i + 2; j < path.Count - 2; j++)
                    {
                        double dist = Params.Distances[path[i], path[j]] + Params.Distances[path[i + 1], path[j + 1]] - Params.Distances[path[i], path[i + 1]] - Params.Distances[path[j], path[j + 1]];
                        if (dist < minChange)
                        {
                            minChange = dist;
                            minI = i;
                            minJ = j;
                        }
                    }
                }
                iterations++;
                if (minChange < 0)
                {
                    path.Points = modifyPath(path.Points, minI, minJ);
                    path.Length += minChange;
                }
            } while (minChange < 0 && iterations < limit);
        }
        /*
        public List<int> modifyPath(List<int> path, int lT, int rT)
        {
            List<int> modifiedPath = new List<int>();
            for (int i = 0; i <= lT; i++)
            {
                modifiedPath.Add(path[i]);
            }
            for (int i = rT; i > lT; i--)
            {
                modifiedPath.Add(path[i]);
            }
            for (int i = rT + 1; i < path.Count; i++)
            {
                modifiedPath.Add(path[i]);
            }

            return modifiedPath;
        }
        */

        public List<int> modifyPath(List<int> path, int lT, int rT)
        {
            List<int> modifiedPath = new List<int>(path);
            int tmp;
            lT++;
            while (lT < rT)
            {
                tmp = modifiedPath[lT];
                modifiedPath[lT] = modifiedPath[rT];
                modifiedPath[rT] = tmp;
                lT++;
                rT--;
            }
            return modifiedPath;
        }

    }
}
