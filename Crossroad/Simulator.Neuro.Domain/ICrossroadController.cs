namespace Simulator.Neuro.Domain
{
    public interface ICrossroadController
    {
        void Step();
        void Reinforce(double value);
        double[,] W0 { get; set; }
    }
}