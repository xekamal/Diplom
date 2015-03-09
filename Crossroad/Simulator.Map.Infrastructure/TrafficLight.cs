namespace Simulator.Map.Infrastructure
{
    public class TrafficLight : ITrafficLight
    {
        public TrafficLight()
        {
            State = TrafficLightState.Red;
        }

        public TrafficLightState State { get; set; }
    }
}