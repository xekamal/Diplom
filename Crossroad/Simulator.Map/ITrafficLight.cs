using System;

namespace Simulator.Map
{
    public interface ITrafficLight : ICloneable
    {
         TrafficLightState State { get; set; }
    }
}