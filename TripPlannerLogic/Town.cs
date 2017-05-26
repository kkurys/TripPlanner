using System;

namespace Genetic_V8
{
    public class Town
    {
        public double x
        {
            set;
            get;
        }
        public double y
        {
            set;
            get;
        }
        public double profit
        {
            set;
            get;
        }
        public Town(double px, double py, double p)
        {
            x = px;
            y = py;
            profit = p;
        }
        public int calculateDistanceToOtherTown(Town t1)
        {
            return (int)Math.Floor(Math.Sqrt((x - t1.x) * (x - t1.x) + (y - t1.y) * (y - t1.y)));
        }
        public double GetGeoDistanceToPoint(Town t1)
        {
            return Geolocation.GeoCalculator.GetDistance(x, y, t1.x, t1.y, 3, Geolocation.DistanceUnit.Meters)/1000;
        }
    }
}
