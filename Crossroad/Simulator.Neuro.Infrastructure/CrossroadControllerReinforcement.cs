using Simulator.Map;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{
    public class CrossroadControllerReinforcement : ICrossroadController
    {
        private readonly ICrossroad _crossroad;

        public CrossroadControllerReinforcement(ICrossroad crossroad)
        {
            _crossroad = crossroad;
        }

        public void Step()
        {
            throw new System.NotImplementedException();
        }

        public void Reinforce(double value)
        {
            throw new System.NotImplementedException();
        }
    }
}