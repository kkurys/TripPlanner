namespace TripPlannerLogic
{
    public class Route
    {
        public double Value { get; set; }
        public int Point { get; set; }
        public Route(double pValue, int pPoint)
        {
            Value = pValue;
            Point = pPoint;
        }
    }
}
