using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TripPlannerLogic;

namespace TripPlannerTests
{
    [TestClass]
    public class DistanceCalculatorsTests
    {
        [TestMethod]
        public void point_distance_to_works_in_euclidean()
        {
            RoutePoint point1 = new RoutePoint(0, 0, 0);
            RoutePoint point2 = new RoutePoint(3, 4, 0);
            RoutePoint point3 = new RoutePoint(3, 5, 0);

            Assert.AreEqual(5, point1.GetEuclideanDistanceToPoint(point2));
            Assert.AreEqual(5, point2.GetEuclideanDistanceToPoint(point1));

            // should be approxed to floor..
            Assert.AreEqual(5, point1.GetEuclideanDistanceToPoint(point3));
            Assert.AreEqual(5, point2.GetEuclideanDistanceToPoint(point1));
        }
        [TestMethod]
        public void point_distance_to_works_in_geo_data()
        {
            RoutePoint point1 = new RoutePoint(54.23843615, 23.20775055, 0);
            RoutePoint point2 = new RoutePoint(53.22801039, 21.87498746, 0);

            Assert.AreEqual(Math.Floor(142.76), Math.Floor(point1.GetGeoDistanceToPoint(point2)));
            Assert.AreEqual(Math.Floor(142.76), Math.Floor(point2.GetGeoDistanceToPoint(point1)));
        }
    }
}
