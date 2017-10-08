using Genetic_V8;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TripPlanner.BingServices;
using TripPlanner.Business;
using TripPlanner.Core;
using TripPlanner.Support;

namespace TripPlanner
{

    public partial class MainWindow : Window
    {
        TripPlannerOld _tripPlanner = new TripPlannerOld();
        SolutionValidator _validator = new SolutionValidator();
        DataReader _dataReader = new DataReader();
        ObservableCollection<Hotel> _hotels;
        List<Attraction> _attractions;
        Dictionary<int, List<Pushpin>> _dayAttractions;
        Dictionary<int, MapPolyline> _dayRealRoads;
        Dictionary<int, MapPolyline> _dayStraightLines;
        MapOptionsViewModel _mapOptionsViewModel = new MapOptionsViewModel();
        SettingsViewModel _settingsViewModel = new SettingsViewModel();
        InputViewModel _inputViewModel = new InputViewModel();
        private Object _lock = new Object();
        private ObservableCollection<DayTripPlan> _tripPlan;
        private BingServices.RouteResult _routeResult;
        private System.Diagnostics.Stopwatch watch;
        public BingServices.RouteResult RouteResult
        {
            get { return _routeResult; }
            set
            {
                _routeResult = value;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            _hotels = new ObservableCollection<Hotel>(_dataReader.ReadHotels("hotele.txt"));
            _attractions = _dataReader.ReadAttractions("grafy_dane.txt");
            _tripPlan = new ObservableCollection<DayTripPlan>();
            _dayAttractions = new Dictionary<int, List<Pushpin>>();
            _dayRealRoads = new Dictionary<int, MapPolyline>();
            _dayStraightLines = new Dictionary<int, MapPolyline>();
            LBTripDays.ItemsSource = _tripPlan;
            CBHotels.ItemsSource = _hotels;
            _mapOptionsViewModel.PropertyChanged += _mapOptionsViewModel_PropertyChanged;
            _settingsViewModel.OldExecutionTime = Parameters.ExecutionTime;
            _settingsViewModel.ExecutionTime = Parameters.ExecutionTime;
            InputGrid.DataContext = _inputViewModel;
            GBDisplay.DataContext = _mapOptionsViewModel;
            GBSettings.DataContext = _settingsViewModel;
            PlotHotels();

        }

        private void UpdateTripPlan()
        {
            _tripPlan.Clear();
            for (int i = 0; i < Parameters.solutions.Count; i++)
            {
                var _dayTripPlan = new DayTripPlan();
                var _dayPlan = Parameters.solutions[i].path;
                _dayTripPlan.Day = i + 1;
                _dayTripPlan.Attractions.Add(Hotel.ActiveHotel);
                for (int attr = 1; attr <= Parameters.solutions[i].Count - 2; attr++)
                {
                    var _attraction = _attractions[_dayPlan[attr] - 1];
                    _dayTripPlan.Attractions.Add(new AttractionWrapper(_attraction, attr));
                }
                _tripPlan.Add(_dayTripPlan);
                _dayTripPlan.Attractions.Add(Hotel.ActiveHotel);
            }
        }
        private void ChangeActiveHotel(object sender, EventArgs e)
        {
            if (CBHotels.SelectedIndex == -1) return;
            if (Hotel.ActiveHotel != null)
            {
                Hotel.ActiveHotel.Pin.Background = new SolidColorBrush(Color.FromRgb(100, 100, 200));
                if (!_mapOptionsViewModel.DisplayHotels)
                {
                    Hotel.ActiveHotel.Pin.Visibility = Visibility.Hidden;
                }
            }
            Hotel.ActiveHotel = (CBHotels.SelectedItem as Hotel);
            Hotel.ActiveHotel.Pin.Background = new SolidColorBrush(Color.FromRgb(0, 0, 240));
            Hotel.ActiveHotel.Pin.Visibility = Visibility.Visible;
            ContentPopup.DataContext = null;
            ContentPopup.DataContext = Hotel.ActiveHotel;
        }

        #region mouse events
        private void ShowLocation(object sender, MouseEventArgs e)
        {
            var poi = LBTripPlan.SelectedItem as PointOfInterest;
            if (poi != null)
            {
                BingMap.SetView(new Microsoft.Maps.MapControl.WPF.Location()
                {
                    Latitude = poi.Lat,
                    Longitude = poi.Long
                }, 16);
            }

        }
        private void SetActiveCity(object sender, MouseEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            var defaultColor = pin.Background;
            if (CBHotels.SelectedIndex != -1)
            {
                (CBHotels.SelectedItem as Hotel).Pin.Background = new SolidColorBrush(Color.FromRgb(100, 100, 200));
            }
            pin.Background = new SolidColorBrush(Color.FromRgb(0, 0, 240));
            CBHotels.SelectedItem = GetHotelById((pin.Tag as PointOfInterest).Id);
        }
        private Hotel GetHotelById(int id)
        {
            foreach (Hotel hotel in _hotels)
            {
                if (hotel.Id == id)
                {
                    return hotel;
                }
            }
            return null;
        }
        private void ToggleTimes(object sender, MouseEventArgs e)
        {
            if (GBTimes.Visibility == Visibility.Collapsed)
            {
                GBTimes.Visibility = Visibility.Visible;
            }
            else
            {
                GBTimes.Visibility = Visibility.Collapsed;
            }
        }
        private void Route_MouseLeave(object sender, MouseEventArgs e)
        {
            ContentPopup.Visibility = Visibility.Collapsed;
        }
        private void Route_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement pin = sender as FrameworkElement;
            MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin));
            MapLayer.SetPositionOffset(ContentPopup, new Point(20, -15));

            var location = pin.Tag;
            if (location is Attraction)
            {
                location = location as Attraction;

            }
            else
            {
                location = location as Hotel;
            }
            ContentPopup.DataContext = location;
            ContentPopup.Visibility = Visibility.Visible;
        }
        #endregion
        #region initial plotting
        private void PlotAllPoints()
        {
            for (int i = 0; i < Parameters.numberOfTowns + 1; i++)
            {
                Ellipse e = new Ellipse();
                e.Stroke = Brushes.Gray;
                e.Fill = Brushes.Gray;
                e.Height = Parameters.ProfitModifier * Parameters.profits[i];
                e.Width = Parameters.ProfitModifier * Parameters.profits[i];
                if (i == 0)
                {
                    e.Stroke = Brushes.Gold;
                    e.Fill = Brushes.Gold;
                    e.Height = 10;
                    e.Width = 10;
                }

                canvas.Children.Add(e);
                Canvas.SetTop(e, Parameters.Coordinates[i, 1] * Parameters.DrawModifier - e.Height / 2);
                Canvas.SetLeft(e, Parameters.Coordinates[i, 0] * Parameters.DrawModifier - e.Width / 2);
            }
        }
        private void PlotHotels()
        {
            foreach (Hotel hotel in _hotels)
            {
                Pushpin pin = new Pushpin();
                pin.Location = new Microsoft.Maps.MapControl.WPF.Location(hotel.Lat, hotel.Long);
                pin.Tag = hotel;
                PushPinLayer.Children.Add(pin);
                pin.Content = "H";
                pin.MouseEnter += Route_MouseEnter;
                pin.MouseLeave += Route_MouseLeave;
                pin.MouseDown += SetActiveCity;
                pin.Background = new SolidColorBrush(Color.FromRgb(100, 100, 200));
                hotel.Pin = pin;
            }
        }
        #endregion
        #region benchmark
        private void StartBenchmark(object sender, RoutedEventArgs e)
        {
            Parameters.GetParamsForBenchmark(LBFile.Content.ToString());
            PlotAllPoints();
            Parameters.ChosenStart = Parameters.numberOfTowns + 1;
            Parameters.Notify = UpdateDisplay;
            Parameters.Reset();
            Thread t = new Thread(() => _tripPlanner.GenerateRoutes());
            t.Start();
            watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
        }

        public void LoadFile(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt";


            var result = dlg.ShowDialog();
            switch (result)
            {
                case true:
                    var file = dlg.FileName;
                    LBFile.Content = file;
                    LBFile.ToolTip = file;
                    BTNStartBenchmark.IsEnabled = true;
                    break;
                case false:
                    LBFile.Content = "";
                    BTNStartBenchmark.IsEnabled = false;
                    break;
                default:
                    LBFile.Content = "";
                    BTNStartBenchmark.IsEnabled = false;
                    break;
            }
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


        private void Plot(Individual currentRoute)
        {
            canvas.Children.Clear();
            PlotAllPoints();
            LBRoutes.Content = "";
            //LBFullRoutes.Text = "";
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
                //   LBFullRoutes.Text += "Day: " + x + ": ";
                for (int i = 0; i < Parameters.solutions[x].Count; i++)
                {
                    points[i] = new Point(Parameters.Coordinates[Parameters.solutions[x][i], 0] * Parameters.DrawModifier, Parameters.Coordinates[Parameters.solutions[x][i], 1] * Parameters.DrawModifier);
                    //   LBFullRoutes.Text += Parameters.solutions[x][i].ToString() + " ";
                }
                DrawLine(points, DayColors.GetColor(x));

                LBRoutes.Content += "Day: " + x + "\nLength: " + Parameters.solutions[x].length + " Profit " + Parameters.solutions[x].profit + "\n";
                //    LBFullRoutes.Text += "\n";
            }
            if (Parameters.solutions.Count == Parameters.daysOfTrip)
            {
                watch.Stop();
                LBExecutionTime.Content = (watch.ElapsedMilliseconds / 1000.0 + " s");
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
                points[i] = new Point(Parameters.Coordinates[route[i], 0] * Parameters.DrawModifier, Parameters.Coordinates[route[i], 1] * Parameters.DrawModifier);
            }
            DrawLine(points, DayColors.GetColor(x));
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


        #endregion
        #region displaying

        private void LinesChecked(object sender, EventArgs e)
        {
            HideRoads();
            ShowLines();
        }
        private void RoadsChecked(object sender, EventArgs e)
        {
            HideLines();
            ShowRoads();
        }
        private void HideLines()
        {
            if (Parameters.solutions == null) return;
            for (int i = 1; i <= Parameters.solutions.Count; i++)
            {
                if (_dayStraightLines != null && _dayStraightLines.ContainsKey(i))
                {
                    _dayStraightLines[i].Visibility = Visibility.Hidden;
                }
            }
        }
        private void HideRoads()
        {
            if (Parameters.solutions == null) return;
            for (int i = 1; i <= Parameters.solutions.Count; i++)
            {
                if (_dayRealRoads != null && _dayRealRoads.ContainsKey(i))
                {
                    _dayRealRoads[i].Visibility = Visibility.Hidden;
                }
            }
        }
        private void ShowLines()
        {
            if (Parameters.solutions == null) return;
            for (int i = 1; i <= Parameters.solutions.Count; i++)
            {
                if (!_tripPlan[i - 1].Visible) continue;
                if (_dayStraightLines != null && _dayStraightLines.ContainsKey(i))
                {
                    _dayStraightLines[i].Visibility = Visibility.Visible;
                }
            }
        }
        private void ShowRoads()
        {
            if (Parameters.solutions == null) return;

            for (int i = 1; i <= Parameters.solutions.Count; i++)
            {
                if (!_tripPlan[i - 1].Visible) continue;
                if (_dayRealRoads != null && _dayRealRoads.ContainsKey(i))
                {
                    _dayRealRoads[i].Visibility = Visibility.Visible;
                }
            }
        }

        #endregion
        #region commands
        public void ChangeVisibility_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DayTripPlan day = e.Parameter as DayTripPlan;
            Button btn = e.OriginalSource as Button;

            if (day.Visible)
            {
                if (RBRoads.IsChecked.Value)
                {
                    _dayRealRoads[day.Day].Visibility = Visibility.Hidden;
                }
                else
                {
                    _dayStraightLines[day.Day].Visibility = Visibility.Hidden;
                }
                if (_dayAttractions.ContainsKey(day.Day))
                {
                    foreach (Pushpin pin in _dayAttractions[day.Day])
                    {
                        pin.Visibility = Visibility.Hidden;
                    }
                }
                day.Visible = false;
            }
            else
            {
                if (RBRoads.IsChecked.Value)
                {
                    _dayRealRoads[day.Day].Visibility = Visibility.Visible;
                }
                else
                {
                    _dayStraightLines[day.Day].Visibility = Visibility.Visible;
                }
                if (_dayAttractions.ContainsKey(day.Day))
                {
                    foreach (Pushpin pin in _dayAttractions[day.Day])
                    {
                        pin.Visibility = Visibility.Visible;
                    }
                }
                day.Visible = true;
            }
        }
        private void GeneratePlan_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CBHotels.SelectedIndex != -1)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void GeneratePlan_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Parameters.numberOfTowns = _attractions.Count;
            Parameters.ChosenStart = Hotel.ActiveHotel.Id;
            Parameters.distances = DistanceCalculator.GetDistances(Hotel.ActiveHotel, _attractions);
            Parameters.profits = ProfitCalculator.GetProfits(_attractions);
            Parameters.maxLength = int.Parse(TBLimit.Text);
            Parameters.daysOfTrip = int.Parse(TBDays.Text);
            Parameters.Notify = MockNotify;
            Parameters.Reset();
            var layer = MapInteractivity.GetRouteLineLayer(BingMap);
            layer.Children.Clear();
            PushPinLayer.Children.Clear();
            _dayAttractions.Clear();
            _dayRealRoads.Clear();
            _dayStraightLines.Clear();
            _tripPlan.Clear();
            PlotHotels();
            CBHotels.SelectedItem = Hotel.ActiveHotel;
            Hotel.ActiveHotel.Pin.Background = new SolidColorBrush(Color.FromRgb(0, 0, 240));
            Hotel.ActiveHotel.Pin.Visibility = Visibility.Visible;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Thread t = new Thread(() => _tripPlanner.GenerateRoutes());
            t.Start();
            t.Join();
            watch.Stop();
            LBAlgorithmExecutionTime.Content = watch.ElapsedMilliseconds / 1000.0 + " s";
            UpdateTripPlan();
            GenerateRouteLines();
            DrawLines();
            watch = System.Diagnostics.Stopwatch.StartNew();
            GenerateRoutes();
            DrawRoutes();
            watch.Stop();

            LBMapLoadingExecutionTime.Content = watch.ElapsedMilliseconds / 1000.0 + " s";
        }
        private void SaveSettings_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!HasErrors(SettingsInputGrid))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void SaveSettings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Parameters.ExecutionTime = _settingsViewModel.ExecutionTime;

            _settingsViewModel.OldExecutionTime = _settingsViewModel.ExecutionTime;
        }
        private void ClearSolutions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dayAttractions != null && _dayAttractions.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void ClearSolutions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Parameters.Reset();
            var layer = MapInteractivity.GetRouteLineLayer(BingMap);
            layer.Children.Clear();
            PushPinLayer.Children.Clear();
            _dayAttractions.Clear();
            _dayRealRoads.Clear();
            _dayStraightLines.Clear();
            _tripPlan.Clear();
            CBHotels.SelectedIndex = -1;
            Hotel.ActiveHotel = null;
            PlotHotels();
        }
        #region route generation

        private void GenerateRouteLines()
        {
            for (int n = 1; n <= Parameters.solutions.Count; n++)
            {
                if (!_dayStraightLines.ContainsKey(n))
                {
                    _dayStraightLines.Add(n, new MapPolyline()
                    {
                        Locations = new LocationCollection(),
                        Opacity = 0.65,
                        Stroke = DayColors.GetColor(n),
                        StrokeThickness = 5.0
                    });
                }

                for (int i = 0; i < Parameters.solutions[n - 1].Count; i++)
                {
                    if (i == 0 || i == Parameters.solutions[n - 1].Count - 1)
                    {
                        _dayStraightLines[n].Locations.Add(new Microsoft.Maps.MapControl.WPF.Location()
                        {
                            Latitude = Hotel.ActiveHotel.Lat,
                            Longitude = Hotel.ActiveHotel.Long
                        });
                    }
                    else
                    {
                        var _attraction = _attractions[Parameters.solutions[n - 1][i] - 1];
                        _dayStraightLines[n].Locations.Add(new Microsoft.Maps.MapControl.WPF.Location()
                        {
                            Latitude = _attraction.Lat,
                            Longitude = _attraction.Long
                        });
                        AddAttractionPushpin(_attraction, n, i);
                    }
                }
            }
        }
        private void DrawLines()
        {
            var routeLineLayer = MapInteractivity.GetRouteLineLayer(BingMap);
            if (routeLineLayer == null)
            {
                routeLineLayer = new MapLayer();
                MapInteractivity.SetRouteLineLayer(BingMap, routeLineLayer);
            }
            for (int n = 1; n <= Parameters.solutions.Count; n++)
            {
                routeLineLayer.Children.Add(_dayStraightLines[n]);
            }
        }
        private void AddAttractionPushpin(Attraction _attraction, int n, int content)
        {
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(_attraction.Lat, _attraction.Long);
            pin.Tag = _attraction;
            pin.Content = content;
            PushPinLayer.Children.Add(pin);
            pin.MouseEnter += Route_MouseEnter;
            pin.MouseLeave += Route_MouseLeave;
            pin.Background = DayColors.GetColor(n);
            if (!_dayAttractions.ContainsKey(n))
            {
                _dayAttractions.Add(n, new List<Pushpin>());
            }
            _dayAttractions[n].Add(pin);

        }

        private void GenerateRoutes()
        {
            GetRoutesFromService();
            Application.Current.Dispatcher.BeginInvoke(new Action(() => _mapOptionsViewModel.RoadsReady = true));


        }
        private void GetRoutesFromService()
        {
            using (BingServices.RouteServiceClient client = new BingServices.RouteServiceClient("CustomBinding_IRouteService"))
            {
                int n = 1;
                var credentials = new Credentials() { ApplicationId = (App.Current.Resources["MyCredentials"] as ApplicationIdCredentialsProvider).ApplicationId };

                BingServices.RouteRequest request = new BingServices.RouteRequest();
                request.Credentials = credentials;
                request.Waypoints = new ObservableCollection<Waypoint>();
                foreach (Individual i in Parameters.solutions)
                {
                    int waypointsAdded = 0;

                    request.Waypoints = new ObservableCollection<Waypoint>();
                    for (int x = 0; x < i.Count; x++)
                    {
                        if (x == 0 || x == i.path.Count - 1)
                        {
                            request.Waypoints.Add(
                                    new Waypoint()
                                    {
                                        Location = new BingServices.Location()
                                        {
                                            Latitude = Hotel.ActiveHotel.Lat,
                                            Longitude = Hotel.ActiveHotel.Long
                                        }
                                    });
                            waypointsAdded++;
                        }
                        else
                        {
                            var _attraction = _attractions.Find(z => z.Id == i.path[x]);
                            request.Waypoints.Add(
                                new Waypoint()
                                {
                                    Location = new BingServices.Location()
                                    {
                                        Latitude = _attraction.Lat,
                                        Longitude = _attraction.Long
                                    }
                                });
                            waypointsAdded++;

                        }
                        if (i.Count - x < 3 && waypointsAdded + i.Count - x < 2 || waypointsAdded == 25 || x == i.path.Count - 1)
                        {
                            request.Options = new BingServices.RouteOptions();
                            request.Options.RoutePathType = BingServices.RoutePathType.Points;
                            if (request.Waypoints.Count == 1)
                            {
                                request.Waypoints.Add(
                                     new Waypoint()
                                     {
                                         Location = new BingServices.Location()
                                         {
                                             Latitude = Hotel.ActiveHotel.Lat,
                                             Longitude = Hotel.ActiveHotel.Long
                                         }
                                     });
                            }
                            RouteResult = client.CalculateRoute(request).Result;
                            AddRoutePart(RouteResult, n);
                            request.Waypoints = new ObservableCollection<Waypoint>();
                            waypointsAdded = 0;
                        }
                    }
                    n++;
                }

            }

        }
        private void AddRoutePart(RouteResult path, int n)
        {
            if (!_dayRealRoads.ContainsKey(n))
            {
                _dayRealRoads.Add(n, new MapPolyline()
                {
                    Locations = new LocationCollection(),
                    Opacity = 0.65,
                    Stroke = DayColors.GetColor(n),
                    StrokeThickness = 5.0,
                    Visibility = Visibility.Hidden
                });
            }

            foreach (BingServices.Location loc in path.RoutePath.Points)
            {
                _dayRealRoads[n].Locations.Add(new Microsoft.Maps.MapControl.WPF.Location(loc.Latitude, loc.Longitude));
            }

        }
        private void DrawRoutes()
        {
            var routeLineLayer = MapInteractivity.GetRouteLineLayer(BingMap);
            if (routeLineLayer == null)
            {
                routeLineLayer = new MapLayer();
                MapInteractivity.SetRouteLineLayer(BingMap, routeLineLayer);
            }

            for (int n = 1; n <= Parameters.solutions.Count; n++)
            {
                if (_dayRealRoads.ContainsKey(n))
                {
                    routeLineLayer.Children.Add(_dayRealRoads[n]);
                }
            }

        }
        #endregion
        #endregion
        #region notify & update
        private void MockNotify(Individual i) { }
        private void UpdateDisplay(Individual route)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                ValidateSolution();
                Plot(route);
            });

        }

        #endregion
        #region validation
        private bool HasErrors(DependencyObject gridInfo)
        {
            foreach (object child in LogicalTreeHelper.GetChildren(gridInfo))
            {
                TextBox element = child as TextBox;
                if (element == null)
                {
                    continue;
                }
                if (Validation.GetHasError(element) || HasErrors(element))
                {
                    return true;
                }
            }
            return false;
        }
        private void InputValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                BTNStartPlanner.IsEnabled = false;
            }
            else if (!HasErrors(InputGrid))
            {
                BTNStartPlanner.IsEnabled = true;
            }
        }
        private void SettingsInputValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                BTNSaveSettings.IsEnabled = false;
            }
            else if (!HasErrors(SettingsInputGrid))
            {
                BTNSaveSettings.IsEnabled = true;
            }
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TBDays.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            TBLimit.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
        private void _mapOptionsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DisplayHotels")
            {
                if (!_mapOptionsViewModel.DisplayHotels)
                {
                    foreach (Hotel h in _hotels)
                    {
                        if (h == Hotel.ActiveHotel)
                        {
                            continue;
                        }
                        h.Pin.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    foreach (Hotel h in _hotels)
                    {
                        h.Pin.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
