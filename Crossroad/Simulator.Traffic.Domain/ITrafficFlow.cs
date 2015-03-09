using System.Collections.Generic;
using Simulator.Map;

namespace Simulator.Traffic.Domain
{
    public interface ITrafficFlow
    {
        double TrafficSpeed { get; set; }
        double TrafficDensity { get; set; }
        IList<ILocation> Path { get; set; }
    }
}