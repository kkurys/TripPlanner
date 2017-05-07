using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class PointValuePairSortedSet : CustomContainer<PointValuePair>
    {
        public PointValuePairSortedSet(int pSize)
        {
            _size = pSize;
            _container = new List<PointValuePair>();
        }
        public override void Add(PointValuePair item)
        {
            if (_container.Count < _size)
            {
                _container.Add(item);
            }
            else if (item.Value > _container[_size - 1].Value)
            {
                _container[_size - 1] = item;
            }
            else return;

            for (int i = _container.Count - 1; i > 0; i--)
            {
                if (_container[i - 1].Value < _container[i].Value)
                {
                    var tmp = _container[i];
                    _container[i] = _container[i - 1];
                    _container[i - 1] = tmp;
                }
            }
        }
    }
}
