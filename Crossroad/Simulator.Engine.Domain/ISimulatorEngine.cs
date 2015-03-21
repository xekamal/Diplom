namespace Simulator.Engine.Domain
{
    public interface ISimulatorEngine
    {
        void Step(double seconds);
        void Step(double seconds, int nofSteps);
    }
}