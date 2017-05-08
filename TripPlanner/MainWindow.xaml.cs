using System;
using System.Threading;
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
            Params.InitParams("F.txt");
            PlotAllPoints();
            StartPlanner();
            Results.Notify += UpdateDisplay;
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
        private void Plot(Route currentRoute)
        {
            canvas.Children.Clear();
            PlotAllPoints();
            LBRoutes.Content = "";
            LBFullRoutes.Text = "";
            LBProfit.Content = Results.TotalProfit;
            LBLength.Content = Results.TotalLength;
            PlotCurrentSolutions();

            if (currentRoute == null) return;
            PlotRoute(Results.CurrentBestOne, -1);
            PlotRoute(currentRoute, Results.Solutions.Count);


        }

        private void PlotCurrentSolutions()
        {
            for (int x = 0; x < Results.Solutions.Count; x++)
            {
                Point[] points = new Point[Results.Solutions[x].Count];
                LBFullRoutes.Text += "Day: " + x + ": ";
                for (int i = 0; i < Results.Solutions[x].Count; i++)
                {
                    points[i] = new Point(Params.Coordinates[Results.Solutions[x][i], 0] / 2, Params.Coordinates[Results.Solutions[x][i], 1] / 2);
                    LBFullRoutes.Text += Results.Solutions[x][i].ToString() + " ";
                }
                DrawLine(points, GetColor(x));

                LBRoutes.Content += "Day: " + x + "\nLength: " + Results.Solutions[x].Length + " Profit " + Results.Solutions[x].Profit + "\n";
                LBFullRoutes.Text += "\n";
            }
        }
        private void PlotRoute(Route route, int x)
        {
            Point[] points = new Point[route.Count];
            if (x == -1)
            {
                LBRoutes.Content += "(CURRENT BEST)\nDay: " + Results.Solutions.Count + "\nLength: " + route.Length + " Profit " + route.Profit + "\n";
            }
            else
            {
                LBRoutes.Content += "(CURRENT)\nDay: " + Results.Solutions.Count + "\nLength: " + route.Length + " Profit " + route.Profit + "\n";
            }
            for (int i = 0; i < route.Count; i++)
            {
                points[i] = new Point(Params.Coordinates[route[i], 0] / 2, Params.Coordinates[route[i], 1] / 2);
            }
            DrawLine(points, GetColor(x));
        }
        private void UpdateDisplay(Route route)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                ValidateSolution();
                Plot(route);
            });

        }
        private void StartPlanner()
        {
            Results.Init();
            Thread t = new Thread(() => _tripPlanner.GenerateRoutes());
            t.Start();
        }

        private void RestartAlgorithm(object sender, RoutedEventArgs e)
        {
            StartPlanner();
        }
        private Brush GetColor(int x)
        {
            switch (x)
            {
                case 0:
                    return Brushes.Red;
                case 1:
                    return Brushes.Blue;
                case 2:
                    return Brushes.Violet;
                case 3:
                    return Brushes.Violet;
                case 4:
                    return Brushes.Violet;
                case -1:
                    return Brushes.Goldenrod;
                default:
                    return Brushes.White;
            }


        }
    }
}
