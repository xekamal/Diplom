namespace Simulator.Map.Infrastructure
{
    public class TrafficData : ITrafficData
    {
        public TrafficData()
        {
            TrafficDensity = 0.0;
            TrafficSpeed = 0.0;
        }

        public double TrafficSpeed { get; set; }
        public double TrafficDensity { get; set; }
    }
}