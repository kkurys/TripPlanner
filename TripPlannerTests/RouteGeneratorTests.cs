using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TripPlannerLogic;

namespace TripPlannerTests
{
    [TestClass]
    public class RouteGeneratorTests
    {
        [TestMethod]
        public void route_generator_returns_path_that_ends_and_starts_in_the_same_point()
        {
            RouteGenerator generator = new RouteGenerator();

            var path = generator.GetRoute(0);
            var path2 = generator.GetRoute(3);

            Assert.AreEqual(path[0], path[path.Count - 1]);
            Assert.AreEqual(path2[0], path2[path2.Count - 1]);
        }
        [TestMethod]
        public void route_generator_returns_path_containining_starting_point_with_0_index()
        {
            RouteGenerator generator = new RouteGenerator();

            var path1 = generator.GetRoute(0);
            var path2 = generator.GetRoute(1);
            var path3 = generator.GetRoute(2);

            Assert.AreEqual(true, path1.Contains(0));
            Assert.AreEqual(true, path2.Contains(0));
            Assert.AreEqual(true, path3.Contains(0));
        }
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Params.NumberOfPoints = 5;
            Params.Profits = new double[6] { 0, 1, 2, 3, 4, 5 };
            Params.Distances = new double[6, 6]
            {
                {0, 1, 1, 1, 1, 1 },
                {1, 0, 1, 1, 1, 1 },
                {1, 1, 0, 1, 1, 1 },
                {1, 1, 1, 0, 1, 1 },
                {1, 1, 1, 1, 0, 1 },
                {1, 1, 1, 1, 1, 0 }
            };
            Params.AvailablePoints = new HashSet<int>(new int[] { 1, 2, 3, 4, 5 });
            Params.MaxLength = 5;
        }

    }
}
