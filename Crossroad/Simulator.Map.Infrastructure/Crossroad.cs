using Simulator.Neuro.Domain;
using Simulator.Neuro.Infrastructure;

namespace Simulator.Map.Infrastructure
{
    public class Crossroad : ICrossroad
    {
        public Crossroad()
        {
            CrossroadController = new CrossroadControllerReinforcement(this);

            LeftToUpTrafficLight = new TrafficLight();
            LeftToRightTrafficLight = new TrafficLight();
            LeftToDownTrafficLight = new TrafficLight();
            UpToLeftTrafficLight = new TrafficLight();
            UpToDownTrafficLight = new TrafficLight();
            UpToRightTrafficLight = new TrafficLight();
            RightToUpTrafficLight = new TrafficLight();
            RightToLeftTrafficLight = new TrafficLight();
            RightToDownTrafficLight = new TrafficLight();
            DownToLeftTrafficLight = new TrafficLight();
            DownToUpTrafficLight = new TrafficLight();
            DownToRightTrafficLight = new TrafficLight();

            LeftToUpTrafficData = new TrafficData();
            LeftToRightTrafficData = new TrafficData();
            LeftToDownTrafficData = new TrafficData();
            UpToLeftTrafficData = new TrafficData();
            UpToDownTrafficData = new TrafficData();
            UpToRightTrafficData = new TrafficData();
            RightToUpTrafficData = new TrafficData();
            RightToLeftTrafficData = new TrafficData();
            RightToDownTrafficData = new TrafficData();
            DownToLeftTrafficData = new TrafficData();
            DownToUpTrafficData = new TrafficData();
            DownToRightTrafficData = new TrafficData();

            SetAllTrafficLights(TrafficLightState.Red);
        }

        public double Length { get; set; }
        public ITrafficLight LeftToUpTrafficLight { get; set; }
        public ITrafficLight LeftToRightTrafficLight { get; set; }
        public ITrafficLight LeftToDownTrafficLight { get; set; }
        public ITrafficLight UpToLeftTrafficLight { get; set; }
        public ITrafficLight UpToDownTrafficLight { get; set; }
        public ITrafficLight UpToRightTrafficLight { get; set; }
        public ITrafficLight RightToUpTrafficLight { get; set; }
        public ITrafficLight RightToLeftTrafficLight { get; set; }
        public ITrafficLight RightToDownTrafficLight { get; set; }
        public ITrafficLight DownToLeftTrafficLight { get; set; }
        public ITrafficLight DownToUpTrafficLight { get; set; }
        public ITrafficLight DownToRightTrafficLight { get; set; }
        public ITrafficData LeftToUpTrafficData { get; set; }
        public ITrafficData LeftToRightTrafficData { get; set; }
        public ITrafficData LeftToDownTrafficData { get; set; }
        public ITrafficData UpToLeftTrafficData { get; set; }
        public ITrafficData UpToDownTrafficData { get; set; }
        public ITrafficData UpToRightTrafficData { get; set; }
        public ITrafficData RightToUpTrafficData { get; set; }
        public ITrafficData RightToLeftTrafficData { get; set; }
        public ITrafficData RightToDownTrafficData { get; set; }
        public ITrafficData DownToLeftTrafficData { get; set; }
        public ITrafficData DownToUpTrafficData { get; set; }
        public ITrafficData DownToRightTrafficData { get; set; }
        public ICrossroadController CrossroadController { get; set; }

        private void SetAllTrafficLights(TrafficLightState state)
        {
            LeftToUpTrafficLight.State = state;
            LeftToRightTrafficLight.State = state;
            LeftToDownTrafficLight.State = state;
            UpToLeftTrafficLight.State = state;
            UpToDownTrafficLight.State = state;
            UpToRightTrafficLight.State = state;
            RightToUpTrafficLight.State = state;
            RightToLeftTrafficLight.State = state;
            RightToDownTrafficLight.State = state;
            DownToLeftTrafficLight.State = state;
            DownToUpTrafficLight.State = state;
            DownToRightTrafficLight.State = state;
        }
    }
}