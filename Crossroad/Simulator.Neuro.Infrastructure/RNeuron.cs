using System;
using Simulator.Map;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{
    public class RNeuron : ANeuron
    {
        private readonly int _nOfDendrits;
        public TrafficLightState axonState;
        public double atan;
        public RNeuron(int nOfDendrits)
        {
            _nOfDendrits = nOfDendrits;
            dendrits = new double[_nOfDendrits];
        }

        public override void Activation()
        {
            /*double x = 0;
            for (int i = 0; i <_nOfDendrits; i++)
            {
                x += dendrits[i];
            };
            axonState = TrafficLightState.Red;
            axon = x;*/

            var sum = 0.0;
            for (var i = 0; i < _nOfDendrits; i++)
            {
                sum += dendrits[i];
            }
             atan= Math.Atan(sum)/1.3;
            double T = 0.8;
            if (atan>T)
            {
                axon = 1;
            }
            else
            {
                axon = 0;
            }
            //var atan = Math.Atan(sum)/1.35;
            //axon = atan >= 0 ? 1 : 0;
            //axon = atan;
        }
    }
}