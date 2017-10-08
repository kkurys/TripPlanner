namespace TripPlanner.Business
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double GetGeoDistanceToPoint(PointOfInterest t1)
        {
            return Geolocation.GeoCalculator.GetDistance(Lat, Long, t1.Lat, t1.Long, 3, Geolocation.DistanceUnit.Meters) / 1000;
        }
    }
}
