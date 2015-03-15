using Simulator.Map;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{
    public class RNeuron : ANeuron
    {
        private int _nOfDendrits;
        public TrafficLightState axonState;
        public RNeuron(int nOfDendrits)
        {
            _nOfDendrits = nOfDendrits;
            dendrits = new double[_nOfDendrits];
        }

        public override void Activation()
        {
            double x = 0;
            for (int i = 0; i <_nOfDendrits; i++)
            {
                x += dendrits[i];
            };
            axonState = TrafficLightState.Red;
            axon = x;
        }
    }
}