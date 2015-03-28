namespace Simulator.Map.Infrastructure
{
    public class TrafficLight : ITrafficLight
    {
        public TrafficLight()
        {
            State = TrafficLightState.Red;
        }

        public TrafficLightState State { get; set; }

        public object Clone()
        {
            ITrafficLight copy = new TrafficLight();
            copy.State = State;
            return copy;
        }
    }
}