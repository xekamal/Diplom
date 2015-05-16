namespace Simulator.Neuro.Domain
{
    public interface ICrossroadController
    {
        void Step();
        void Reinforce(double value);
        void Reinforce(double v0, double v1, double v2, double v3, double v4, double v5, double v6, double v7, double v8, double v9, double v10, double v11);
        void Reinforce(double[] values);
    }
}