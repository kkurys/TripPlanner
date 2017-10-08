namespace TripPlanner.Business
{
    public class AttractionWrapper : Attraction
    {
        public AttractionWrapper(Attraction attr, int numberOrder)
        {
            Name = attr.Name;
            Lat = attr.Lat;
            Long = attr.Long;
            Profit = attr.Profit;
            NumberOrder = numberOrder;
            Category = attr.Category;
        }
        public int NumberOrder { get; set; }
        public new string Name
        {
            get
            {
                return string.Format("{0}. {1}", NumberOrder, base.Name);
            }
            set
            {
                base.Name = value;
            }
        }
    }
}
