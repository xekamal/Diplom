namespace Simulator.Map.Infrastructure
{
    public class TrafficData : ITrafficData
    {
        public TrafficData()
        {
            TrafficDensity = 0.0;
            TrafficSpeed = 0.0;
            NofPassingTrafficFlows = 0;
        }

        public double TrafficSpeed { get; set; }
        public double TrafficDensity { get; set; }
        public int NofPassingTrafficFlows { get; set; }

        public object Clone()
        {
            ITrafficData copy = new TrafficData();

            copy.NofPassingTrafficFlows = NofPassingTrafficFlows;
            copy.TrafficDensity = TrafficDensity;
            copy.TrafficSpeed = TrafficSpeed;

            return copy;
        }
    }
}