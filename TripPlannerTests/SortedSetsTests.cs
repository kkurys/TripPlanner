using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripPlannerLogic;

namespace TripPlannerTests
{
    [TestClass]
    public class SortedSetsTests
    {
        [TestMethod]
        public void point_value_set_is_being_correctly_sorted()
        {
            PointValuePairSortedSet set = new PointValuePairSortedSet(3);
            PointValuePair p1 = new PointValuePair(-2, 5);
            PointValuePair p2 = new PointValuePair(-1, 3);
            PointValuePair p3 = new PointValuePair(15, 2);
            PointValuePair p4 = new PointValuePair(10, 7);
            PointValuePair p5 = new PointValuePair(16, 6);

            set.Add(p1);
            set.Add(p2);
            set.Add(p3);
            set.Add(p4);
            set.Add(p5);

            Assert.AreEqual(6, set[0].Point);
            Assert.AreEqual(2, set[1].Point);
            Assert.AreEqual(7, set[2].Point);
        }
    }
}
