namespace Simulator.Traffic.Domain
{
    public interface ITrafficManager
    {
        void AddTrafficFlow(ITrafficFlow trafficFlow);
        void CalculateTrafficData();
        void CalculateTrafficData(double seconds);
        void CalculateNofPassedCars(double seconds);
        void BeginSwitchTrafficLights();
        void EndSwitchTrafficLights();
    }
}