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
            _trafficManager.SwitchTrafficLights();

        }

        public void Step(double seconds, int nofSteps)
        {
            throw new System.NotImplementedException();
        }
    }
}