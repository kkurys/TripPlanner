using System.Windows.Input;

namespace TripPlanner
{
    public static class Commands
    {
        public static readonly RoutedUICommand ChangeVisibility = new RoutedUICommand("ChangeVisibility", "ChangeVisibility", typeof(Commands));
        public static readonly RoutedUICommand GeneratePlan = new RoutedUICommand("GeneratePlan", "GeneratePlan", typeof(Commands));
        public static readonly RoutedUICommand ClearSolutions = new RoutedUICommand("ClearSolutions", "ClearSolutions", typeof(Commands));
        public static readonly RoutedUICommand SaveSettings = new RoutedUICommand("SaveSettings", "SaveSettings", typeof(Commands));
    }
}
