using Microsoft.Maps.MapControl.WPF;
using System.Text.RegularExpressions;
using System.Windows;

namespace TripPlanner.Core
{
    public class MapInteractivity
    {

        #region RouteResult

        public static readonly DependencyProperty RouteResultProperty = DependencyProperty.RegisterAttached("RouteResult", typeof(BingServices.RouteResult), typeof(MapInteractivity), new UIPropertyMetadata(null, OnRouteResultChanged));
        public static BingServices.RouteResult GetRouteResult(DependencyObject target)
        {
            return (BingServices.RouteResult)target.GetValue(RouteResultProperty);
        }
        public static void SetRouteResult(DependencyObject target, BingServices.RouteResult value)
        {
            target.SetValue(RouteResultProperty, value);
        }

        private static void OnRouteResultChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnRouteResultChanged((Map)o, (BingServices.RouteResult)e.OldValue, (BingServices.RouteResult)e.NewValue);
        }

        private static void OnRouteResultChanged(Map map, BingServices.RouteResult oldValue, BingServices.RouteResult newValue)
        {
        }

        #endregion //RouteResult

        #region RouteLineLayer

        public static readonly DependencyProperty RouteLineLayerProperty = DependencyProperty.RegisterAttached("RouteLineLayer", typeof(MapLayer), typeof(MapInteractivity), new UIPropertyMetadata(null, OnRouteLineLayerChanged));
        public static MapLayer GetRouteLineLayer(DependencyObject target)
        {
            return (MapLayer)target.GetValue(RouteLineLayerProperty);
        }
        public static void SetRouteLineLayer(DependencyObject target, MapLayer value)
        {
            target.SetValue(RouteLineLayerProperty, value);
        }
        private static void OnRouteLineLayerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnRouteLineLayerChanged((Map)o, (MapLayer)e.OldValue, (MapLayer)e.NewValue);
        }
        private static void OnRouteLineLayerChanged(Map map, MapLayer oldValue, MapLayer newValue)
        {
            if (!map.Children.Contains(newValue))
                map.Children.Add(newValue);
        }
        private static string GetDirectionText(BingServices.ItineraryItem item)
        {
            string contentString = item.Text;
            //Remove tags from the string
            Regex regex = new Regex("<(.|\n)*?>");
            contentString = regex.Replace(contentString, string.Empty);
            return contentString;
        }
        #endregion //RouteLineLayer
    }
}
