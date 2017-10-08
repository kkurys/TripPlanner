using System.ComponentModel;

namespace TripPlanner.Core
{
    public class MapOptionsViewModel : INotifyPropertyChanged
    {
        private bool _roadsReady, _displayHotels;
        public MapOptionsViewModel()
        {
            _roadsReady = false;
            _displayHotels = true;
        }
        public bool RoadsReady
        {
            get
            {
                return _roadsReady;
            }
            set
            {
                _roadsReady = value;
                onPropertyChanged(this, "RoadsReady");
            }
        }
        public bool DisplayHotels
        {
            get
            {
                return _displayHotels;
            }
            set
            {
                _displayHotels = value;
                onPropertyChanged(this, "DisplayHotels");
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
