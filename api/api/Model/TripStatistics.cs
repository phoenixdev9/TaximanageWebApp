namespace api.Model
{
    public class TripStatistics
    {
        public double TotalTrips { get; set; }
        public double TotalRevenue { get; set; }
        public double AverageTripDistance { get; set; }
        public double AverageFareAmount { get; set; }
        public Dictionary<string, int> PaymentTypeDistribution { get; set; } = new Dictionary<string, int>();
        public Dictionary<int, int> PassengerCountDistribution { get; set; } = new Dictionary<int, int>();
        public Dictionary<double, int> TipDistribution { get; set; } = new Dictionary<double, int>();
        public Dictionary<string, int> TipPercentageDistribution { get; set; } = new Dictionary<string, int>();
    }
}