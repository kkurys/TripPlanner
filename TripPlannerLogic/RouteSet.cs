using System.Collections;

namespace TripPlannerLogic
{
    public class RouteSet : IEnumerable
    {
        public RouteSortedSet setOfRoutes;

        public int Count
        {
            get
            {
                return setOfRoutes.Count;
            }
        }
        public Route this[int index]
        {
            get
            {
                return setOfRoutes[index];
            }
        }
        public RouteSet(int s)
        {
            setOfRoutes = new RouteSortedSet(s);
        }
        public void Add(Route route)
        {
            setOfRoutes.Add(route);

            if (route.Profit >= Results.CurrentBestOne.Profit && route.Length <= Params.MaxLength && route.Contains(0))
            {
                Results.CurrentBestOne = route;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)setOfRoutes).GetEnumerator();
        }
    }
}
