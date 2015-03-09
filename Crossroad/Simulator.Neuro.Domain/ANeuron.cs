namespace Simulator.Neuro.Domain
{
    public abstract class ANeuron : INeuron
    {
        public double axon { get; set; }
        public double[] dendrits;
        protected double _memAxon;
        public abstract void Activation();

    }
}