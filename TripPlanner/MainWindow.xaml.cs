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
        private int _dayOfTrip;
        private Route _bestOne;
        public int DayOfTrip
        {
            get
            {
                return _dayOfTrip;
            }
            set
            {
                _dayOfTrip = value;
            }
        }
        public Route BestOne
        {
            get
            {
                return _bestOne;
            }
            set
            {
                _bestOne = value;
                Plot();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Params.InitParams("F.txt");
            Results.NotifyDay += UpdateDayOfTrip;
            StartPlanner();
            Results.Notify += Update;

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
        private void Plot()
        {
            canvas.Children.Clear();
            PlotAllPoints();
            LBRoutes.Content = "";
            LBFullRoutes.Text = "";
            if (Results.Solutions.Count == 0) return;
            for (int x = 0; x < DayOfTrip; x++)
            {
                Point[] points = new Point[Results.Solutions[x].Count];
                LBFullRoutes.Text += "Day: " + x + ": ";
                for (int i = 0; i < Results.Solutions[x].Count; i++)
                {
                    points[i] = new Point(Params.Coordinates[Results.Solutions[x][i], 0] / 2, Params.Coordinates[Results.Solutions[x][i], 1] / 2);
                    LBFullRoutes.Text += Results.Solutions[x][i].ToString() + " ";
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
                LBFullRoutes.Text += "\n";
            }
            if (DayOfTrip < Params.DaysOfTrip + 1)
            {
                Point[] pointsx = new Point[Results.CurrentBestOne.Count];
                LBFullRoutes.Text += "Day: " + DayOfTrip + ": ";

                for (int i = 0; i < Results.CurrentBestOne.Count; i++)
                {
                    pointsx[i] = new Point(Params.Coordinates[Results.CurrentBestOne[i], 0] / 2, Params.Coordinates[Results.CurrentBestOne[i], 1] / 2);
                    LBFullRoutes.Text += Results.CurrentBestOne[i].ToString() + " ";
                }
                DrawLine(pointsx, Brushes.Blue);

            }
            LBRoutes.Content += "Day: " + DayOfTrip + "\nLength: " + Results.CurrentBestOne.Length + " Profit " + Results.CurrentBestOne.Profit + "\n";
            LBProfit.Content = Results.TotalProfit;
            LBLength.Content = Results.TotalLength;

        }
        private void Update()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                ValidateSolution();
                Plot();
            });
            Thread.Sleep(200);

        }
        private void UpdateDayOfTrip(int d)
        {
            DayOfTrip = d;
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
    }
}
