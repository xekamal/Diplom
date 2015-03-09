namespace Simulator.Traffic.Domain
{
    public interface ITrafficManager
    {
        void AddTrafficFlow(ITrafficFlow trafficFlow);
    }
}