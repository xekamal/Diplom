using Simulator.Engine.Domain;
using Simulator.Traffic.Domain;

namespace Simulator.Engine.Infrastructure
{
    public class SimulatorEngine : ISimulatorEngine
    {
        private ITrafficManager _trafficManager;

        public SimulatorEngine(ITrafficManager trafficManager)
        {
            _trafficManager = trafficManager;
            _trafficManager.CalculateTrafficData();
        }

        public void Step(double seconds)
        {
            _trafficManager.BeginSwitchTrafficLights();
            _trafficManager.CalculateTrafficData(seconds);
            _trafficManager.EndSwitchTrafficLights();
            _trafficManager.CalculateNofPassedCars(seconds);
        }

/*        public void Step(double seconds, int nofSteps)
        {
            throw new System.NotImplementedException();
        }*/
        public void ThreadStep()
        {
            Step(60);
        }
    }
}