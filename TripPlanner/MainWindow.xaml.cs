using System.Windows;
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
            }
            LBProfit.Content = Results.TotalProfit;
            LBLength.Content = Results.TotalLength;
            //DrawLine2(points);
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
                if (myline.X2 == 0 && myline.Y2 == 0) continue;
                canvas.Children.Add(myline);
            }
        }

        private void DrawLine2(Point[] points)
        {
            Polyline line = new Polyline();
            PointCollection collection = new PointCollection();
            foreach (Point p in points)
            {
                collection.Add(p);
            }
            line.Points = collection;
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = 1;
            canvas.Children.Add(line);
        }
    }
}
