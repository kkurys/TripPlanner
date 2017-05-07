using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class Route
    {
        public List<int> Points { get; set; }
        public double Profit { get; set; }
        public double Length { get; set; }
        public double Fitness
        {
            get
            {
                if (Length > 1.3 * Params.MaxLength)
                    return -1;
                else
                    return (Profit * Profit * Profit / Length) * (Params.MaxLength * Profit / Length) * (Params.MaxLength * Profit / Length) * (Params.MaxLength / Length) * (Params.MaxLength / Length);
            }
        }
    }
}
