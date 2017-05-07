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

        public MainWindow()
        {
            InitializeComponent();
            canvas.Children.Clear();
            Params.InitParams("F.txt");
            RouteGenerator routeGenerator = new RouteGenerator();
            PlotAllPoints();
            for (int x = 0; x < Params.DaysOfTrip; x++)
            {
                Route r = routeGenerator.GetRoute(0);
                RouteCalculator.CalculateRouteProfitAndLength(r);
                Results.TotalLength += r.Length;
                Results.TotalProfit += r.Profit;
                Results.Solutions.Add(r);

                Point[] points = new Point[r.Count];
                for (int i = 0; i < r.Count; i++)
                {
                    points[i] = new Point(Params.Coordinates[r[i], 0] / 2, Params.Coordinates[r[i], 1] / 2);
                    Params.AvailablePoints.Remove(r[i]);
                }
                if (x == 0)
                {
                    DrawLine(points, Brushes.Red);
                }
                else if (x == 1)
                {
                    DrawLine(points, Brushes.Blue);
                }
                LBRoutes.Content += "Day: " + x + "\nLength: " + r.Length + " Profit " + r.Profit + "\n";
            }
            LBProfit.Content = Results.TotalProfit;
            LBLength.Content = Results.TotalLength;
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
        public void PlotAllPoints()
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
