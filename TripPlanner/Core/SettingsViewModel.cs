using System.ComponentModel;

namespace TripPlanner.Core
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private int _oldExecutionTime, _executionTime;
        public int OldExecutionTime
        {
            get
            {
                return _oldExecutionTime;
            }
            set
            {
                _oldExecutionTime = value;
                onPropertyChanged(this, "OldExecutionTime");
            }
        }
        public int ExecutionTime
        {
            get
            {
                return _executionTime;
            }
            set
            {
                _executionTime = value;
                onPropertyChanged(this, "ExecutionTime");
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
