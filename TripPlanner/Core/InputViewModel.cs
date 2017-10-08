using System.ComponentModel;

namespace TripPlanner.Core
{
    public class InputViewModel : INotifyPropertyChanged
    {
        private int _limit = 100, _daysOftrip = 1;
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value;
                onPropertyChanged(this, "Limit");
            }
        }
        public int DaysOfTrip
        {
            get
            {
                return _daysOftrip;
            }
            set
            {
                _daysOftrip = value;
                onPropertyChanged(this, "DaysOfTrip");
            }
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
