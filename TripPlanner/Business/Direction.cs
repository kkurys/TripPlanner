using Microsoft.Maps.MapControl.WPF;

namespace TripPlanner.Business
{
    public class Direction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
    }
}
