using System;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{
    public class HNeuron : ANeuron
    {
        private int _nOfDendrits;

        public HNeuron(int nOfDendrits)
        {
            _nOfDendrits = nOfDendrits;
            dendrits = new double[_nOfDendrits];
        }

        public override void Activation()
        {
            double X = 0;
            for (int i = 0; i < _nOfDendrits; i++)
            {
                if (dendrits != null) X += dendrits[i];
            };
            //	X=0-X;
 //           double a = 1.57;
            double a = 1.57;
            double e = 2.71828;
            //	float result = 1/(1+pow(e,X*a));
            double result = Math.Atan(X) / a;
            _memAxon = axon;
            axon=result;
        }
    }
}