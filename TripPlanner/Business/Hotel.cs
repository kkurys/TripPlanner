using Microsoft.Maps.MapControl.WPF;

namespace TripPlanner.Business
{
    public class Hotel : PointOfInterest
    {
        private static Hotel _activeHotel;
        public new string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        public string City { get; set; }
        public override string ToString()
        {
            return City + ", " + Name;
        }
        public Pushpin Pin { get; set; }
        public static Hotel ActiveHotel
        {
            get
            {
                return _activeHotel;
            }
            set
            {
                _activeHotel = value;
            }
        }
    }
}
