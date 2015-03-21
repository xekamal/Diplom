namespace Simulator.Traffic.Domain
{
    public interface ITrafficManager
    {
        void AddTrafficFlow(ITrafficFlow trafficFlow);
        void CalculateTrafficData();
        void CalculateNofPassedCars(double seconds);
        void SwitchTrafficLights();
    }
}