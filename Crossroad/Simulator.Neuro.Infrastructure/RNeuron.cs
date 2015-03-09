using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{
    public class RNeuron : ANeuron
    {
        private int _nOfDendrits;

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
            const double T = 0.3;
            if (x < T)
                axon = 0;
            else
                axon = 1;
        }
    }
}