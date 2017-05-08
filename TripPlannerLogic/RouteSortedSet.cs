using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class RouteSortedSet : CustomContainer<Route>
    {
        public RouteSortedSet(int pSize)
        {
            _size = pSize;
            _container = new List<Route>();
        }
        public override void Add(Route item)
        {
            if (_container.Count < _size)
            {
                _container.Add(item);
            }
            else if (item.Fitness > _container[_size - 1].Fitness)
            {
                _container[_size - 1] = item;
            }
            else return;

            for (int i = _container.Count - 1; i > 0; i--)
            {
                if (_container[i - 1].Fitness < _container[i].Fitness)
                {
                    var tmp = _container[i];
                    _container[i] = _container[i - 1];
                    _container[i - 1] = tmp;
                }
            }
            /*   if ((item.Profit > Results.CurrentBestOne.Profit && item.Length <= Params.MaxLength || (item.Profit == Results.CurrentBestOne.Profit && item.Length < Results.CurrentBestOne.Length && item.Length < Params.MaxLength) && item.Contains(0)))
               {
                   Results.CurrentBestOne = item;
               } */
            if (item.Profit >= Results.CurrentBestOne.Profit && item.Length <= Params.MaxLength && item.Contains(0))
            {
                Results.CurrentBestOne = item;
            }
        }
    }
}