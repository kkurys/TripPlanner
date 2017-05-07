namespace TripPlannerLogic
{
    public class PointValuePair
    {
        public double Value { get; set; }
        public int Point { get; set; }
        public PointValuePair(double pValue, int pPoint)
        {
            Value = pValue;
            Point = pPoint;
        }
    }
}
