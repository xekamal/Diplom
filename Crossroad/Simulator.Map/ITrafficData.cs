using System;

namespace Simulator.Map
{
    public interface ITrafficData : ICloneable
    {
        double TrafficSpeed { get; set; }
        double TrafficDensity { get; set; }
        int NofPassingTrafficFlows { get; set; }
    }
}