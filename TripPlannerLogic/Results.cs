using System.Collections.Generic;

namespace TripPlannerLogic
{
    public static class Results
    {
        private static Route _bestOne { get; set; }
        private static int _dayOftrip { get; set; }
        public static Route CurrentBestOne
        {
            get
            {
                return _bestOne;
            }
            set
            {
                _bestOne = value;
                Notify();
            }
        }
        public static int DayOfTrip
        {
            get
            {
                return _dayOftrip;
            }
            set
            {
                _dayOftrip = value;
                NotifyDay(_dayOftrip);
            }
        }
        public static List<Route> Solutions { get; set; }
        public static double TotalProfit { get; set; }
        public static double TotalLength { get; set; }
        public static HashSet<int> AvailablePoints { get; set; }
        public static Notify Notify;
        public static NotifyDay NotifyDay;
        public static void Init()
        {
            _bestOne = new Route();
            TotalProfit = 0;
            TotalLength = 0;
            DayOfTrip = 0;
            AvailablePoints = new HashSet<int>();
            Solutions = new List<Route>();
            for (int i = 0; i < Params.NumberOfPoints; i++)
            {
                AvailablePoints.Add(i);
            }


        }
    }
    public delegate void Notify();
    public delegate void NotifyDay(int day);
}
