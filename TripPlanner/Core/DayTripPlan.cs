using System.Collections.ObjectModel;
using System.ComponentModel;
using TripPlanner.Business;

namespace TripPlanner.Core
{
    public class DayTripPlan : INotifyPropertyChanged
    {
        public bool _visibility;
        public string Header
        {
            get
            {
                return string.Format("Dzień {0}", Day);
            }
        }
        public int Day { get; set; }
        public ObservableCollection<PointOfInterest> Attractions { get; set; }
        public string VisitedPlaces
        {
            get
            {
                return string.Format("Odwiedzone miejsca: {0}", Attractions.Count - 2);
            }
        }
        public bool Visible
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                onPropertyChanged(this, "Visible");
            }
        }
        public string Profit
        {
            get
            {
                double profit = 0;
                foreach (PointOfInterest poi in Attractions)
                {
                    if (poi is Attraction)
                    {
                        profit += (poi as Attraction).Profit;
                    }
                }
                return string.Format("Atrakcyjność: {0}", profit);
            }
        }
        public DayTripPlan()
        {
            Attractions = new ObservableCollection<PointOfInterest>();
            Visible = true;
        }
        private void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
