using Genetic_V8;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace TripPlanner
{

    public partial class MainWindow : Window
    {
        TripPlannerOld _tripPlanner = new TripPlannerOld();
        SolutionValidator _validator = new SolutionValidator();
        public MainWindow()
        {
            InitializeComponent();
            Parameters.GetParams("grafy_v9_test.txt");
            PlotAllPoints();
            StartPlanner();
            Parameters.Notify += UpdateDisplay;
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
            for (int i = 0; i < Parameters.numberOfTowns + 1; i++)
            {
                Ellipse e = new Ellipse();
                e.Stroke = Brushes.Gray;
                e.Fill = Brushes.Gray;
                e.Height = 12 * Parameters.profits[i] / 5;
                e.Width = 12 * Parameters.profits[i] / 5;
                if (i == 0)
                {
                    e.Stroke = Brushes.Gold;
                    e.Fill = Brushes.Gold;
                    e.Height = 10;
                    e.Width = 10;
                }

                canvas.Children.Add(e);
                //Canvas.SetTop(e, Parameters.Coordinates[i, 1] / 2 - e.Height / 2);
                //Canvas.SetLeft(e, Parameters.Coordinates[i, 0] / 2 - e.Width / 2);
                Canvas.SetTop(e, Parameters.Coordinates[i, 1] * 10 - e.Height / 2);
                Canvas.SetLeft(e, Parameters.Coordinates[i, 0] * 10 - e.Width / 2);
            }
        }
        private void Plot(Individual currentRoute)
        {
            canvas.Children.Clear();
            PlotAllPoints();
            LBRoutes.Content = "";
            LBFullRoutes.Text = "";
            LBProfit.Content = Parameters.totalProfit;
            LBLength.Content = Parameters.totalLength;
            PlotCurrentSolutions();

            if (currentRoute == null) return;
            PlotRoute(Parameters.bestOne, -1);
            PlotRoute(currentRoute, Parameters.solutions.Count);


        }

        private void PlotCurrentSolutions()
        {
            for (int x = 0; x < Parameters.solutions.Count; x++)
            {
                Point[] points = new Point[Parameters.solutions[x].Count];
                LBFullRoutes.Text += "Day: " + x + ": ";
                for (int i = 0; i < Parameters.solutions[x].Count; i++)
                {
                    //     points[i] = new Point(Parameters.Coordinates[Parameters.solutions[x][i], 0] / 2, Parameters.Coordinates[Parameters.solutions[x][i], 1] / 2);
                   points[i] = new Point(Parameters.Coordinates[Parameters.solutions[x][i], 0] * 100, Parameters.Coordinates[Parameters.solutions[x][i], 1] * 100);

                    LBFullRoutes.Text += Parameters.solutions[x][i].ToString() + " ";
                }
                DrawLine(points, GetColor(x));

                LBRoutes.Content += "Day: " + x + "\nLength: " + Parameters.solutions[x].length + " Profit " + Parameters.solutions[x].profit + "\n";
                LBFullRoutes.Text += "\n";
            }
        }
        private void PlotRoute(Individual route, int x)
        {
            Point[] points = new Point[route.Count];
            if (x == -1)
            {
                LBRoutes.Content += "(CURRENT BEST)\nDay: " + Parameters.solutions.Count + "\nLength: " + route.length + " Profit " + route.profit + "\n";
            }
            else
            {
                LBRoutes.Content += "(CURRENT)\nDay: " + Parameters.solutions.Count + "\nLength: " + route.length + " Profit " + route.profit + "\n";
            }
            for (int i = 0; i < route.Count; i++)
            {
                //points[i] = new Point(Parameters.Coordinates[route[i], 0] / 2, Parameters.Coordinates[route[i], 1] / 2);
                points[i] = new Point(Parameters.Coordinates[route[i], 0] * 100, Parameters.Coordinates[route[i], 1] * 100);
            }
            DrawLine(points, GetColor(x));
        }
        private void UpdateDisplay(Individual route)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                ValidateSolution();
                Plot(route);
            });

        }
        private void StartPlanner()
        {
            Parameters.Init();
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
