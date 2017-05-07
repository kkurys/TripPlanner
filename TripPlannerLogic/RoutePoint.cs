using System;

namespace TripPlannerLogic
{
    public class RoutePoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Profit { get; set; }
        public RoutePoint(double pX, double pY, double pProfit)
        {
            X = pX;
            Y = pY;
            Profit = pProfit;
        }
        public int GetEuclideanDistanceToPoint(RoutePoint point)
        {
            return (int)Math.Floor(Math.Sqrt((X - point.X) * (X - point.X) + (Y - point.Y) * (Y - point.Y)));
        }
        public double GetGeoDistanceToPoint(RoutePoint point)
        {
            var p = 0.017453292519943295;    // Math.PI / 180
            var a = 0.5 - Math.Cos((point.X - X) * p) / 2 +
                    Math.Cos(X * p) * Math.Cos(point.X * p) *
                    (1 - Math.Cos((point.Y - Y) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
    }
}
