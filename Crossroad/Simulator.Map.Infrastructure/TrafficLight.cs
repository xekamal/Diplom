namespace Simulator.Map.Infrastructure
{
    public class TrafficLight : ITrafficLight
    {
        public TrafficLight()
        {
            State = TrafficLightState.Red;
            LastStateNofSteps = 0;
        }

        public TrafficLight(TrafficLightState state)
        {
            State = state;
        }

        public TrafficLight(TrafficLightState state, int lastStateNofSteps)
        {
            State = state;
            LastStateNofSteps = lastStateNofSteps;
        }

        public TrafficLightState State { get; private set; }
        public int LastStateNofSteps { get; private set; }

        public void SetTrafficLightState(TrafficLightState state)
        {
            if (State != state)
            {
                LastStateNofSteps = 0;
                State = state;
            }
            else
            {
                LastStateNofSteps++;
            }
        }

        public object Clone()
        {
            return new TrafficLight(State, LastStateNofSteps);
        }
    }
}