using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TripPlannerLogic;

namespace TripPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private RouteGenerator _routeGenerator = new RouteGenerator();
        private SolutionValidator _validator = new SolutionValidator();
        private TripPlannerLogic.TripPlanner _tripPlanner = new TripPlannerLogic.TripPlanner();
        public MainWindow()
        {
            InitializeComponent();
            canvas.Children.Clear();
            Params.InitParams("F.txt");
            PlotAllPoints();
            _tripPlanner.GenerateRoutes();
            for (int x = 0; x < Params.DaysOfTrip; x++)
            {
                Point[] points = new Point[Results.Solutions[x].Count];
                for (int i = 0; i < Results.Solutions[x].Count; i++)
                {
                    points[i] = new Point(Params.Coordinates[Results.Solutions[x][i], 0] / 2, Params.Coordinates[Results.Solutions[x][i], 1] / 2);
                }

                if (x == 0)
                {
                    DrawLine(points, Brushes.Red);
                }
                else if (x == 1)
                {
                    DrawLine(points, Brushes.Blue);
                }
                LBRoutes.Content += "Day: " + x + "\nLength: " + Results.Solutions[x].Length + " Profit " + Results.Solutions[x].Profit + "\n";
            }


            LBProfit.Content = Results.TotalProfit;
            LBLength.Content = Results.TotalLength;
            ValidateSolution();
        }

        private void ValidateSolution()
        {
            ValidateDuplicates();
            ValidateLength();
            ValidateProfit();
        }
        private void ValidateDuplicates()
        {
            if (!_validator.SolutionHasDuplicates())
            {
                LBDuplicatesStatus.Foreground = Brushes.Green;
                LBDuplicatesStatus.Content = "OK";
            }
            else
            {
                LBDuplicatesStatus.Foreground = Brushes.Red;
                LBDuplicatesStatus.Content = "ERROR";
            }
        }
        private void ValidateLength()
        {
            if (!_validator.SolutionLengthIsWrong())
            {
                LBLengthStatus.Foreground = Brushes.Green;
                LBLengthStatus.Content = "OK";
            }
            else
            {
                LBLengthStatus.Foreground = Brushes.Red;
                LBLengthStatus.Content = "ERROR";
            }
        }
        private void ValidateProfit()
        {
            if (!_validator.SolutionTotalProfitIsWrong())
            {
                LBProfitStatus.Foreground = Brushes.Green;
                LBProfitStatus.Content = "OK";
            }
            else
            {
                LBProfitStatus.Foreground = Brushes.Red;
                LBProfitStatus.Content = "ERROR";
            }
        }
        private void DrawLine(Point[] points, Brush color)
        {
            int i;
            int count = points.Length;
            for (i = 0; i < count - 1; i++)
            {
                Line myline = new Line();
                myline.Stroke = color;
                myline.X1 = points[i].X;
                myline.Y1 = points[i].Y;
                myline.X2 = points[i + 1].X;
                myline.Y2 = points[i + 1].Y;
                canvas.Children.Add(myline);
            }
        }
        private void PlotAllPoints()
        {
            for (int i = 0; i < Params.NumberOfPoints + 1; i++)
            {
                Ellipse e = new Ellipse();
                e.Stroke = Brushes.Gray;
                e.Fill = Brushes.Gray;
                e.Height = 6;
                e.Width = 6;
                if (i == 0)
                {
                    e.Stroke = Brushes.Gold;
                    e.Fill = Brushes.Gold;
                    e.Height = 10;
                    e.Width = 10;
                }

                canvas.Children.Add(e);
                Canvas.SetTop(e, Params.Coordinates[i, 1] / 2 - e.Height / 2);
                Canvas.SetLeft(e, Params.Coordinates[i, 0] / 2 - e.Width / 2);
            }
        }
    }
}
