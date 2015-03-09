using System.Collections.Generic;
using Simulator.Map;
using Simulator.Traffic.Domain;

namespace Simulator.Traffic.Infrastructure
{
    public class TrafficFlow : ITrafficFlow
    {
        public double TrafficSpeed { get; set; }
        public double TrafficDensity { get; set; }
        public IList<ILocation> Path { get; set; }

        public TrafficFlow()
        {
            Path = new List<ILocation>();
        }
    }
}