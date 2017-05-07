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
            Route p1 = new Route(-2, 5);
            Route p2 = new Route(-1, 3);
            Route p3 = new Route(15, 2);
            Route p4 = new Route(10, 7);
            Route p5 = new Route(16, 6);

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
