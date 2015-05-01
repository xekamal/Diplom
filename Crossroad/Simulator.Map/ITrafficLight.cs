using System;

namespace Simulator.Map
{
    public interface ITrafficLight : ICloneable
    {
        TrafficLightState State { get; }
        int LastStateNofSteps { get; }
        void SetTrafficLightState(TrafficLightState state);
    }
}