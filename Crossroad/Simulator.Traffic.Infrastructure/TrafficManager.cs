using System.Collections.Generic;
using Simulator.Map;
using Simulator.Traffic.Domain;

namespace Simulator.Traffic.Infrastructure
{
    public class TrafficManager : ITrafficManager
    {
        private readonly IMap _map;
        private readonly IList<ITrafficFlow> _trafficFlows;

        public TrafficManager(IMap map)
        {
            _map = map;
            _trafficFlows = new List<ITrafficFlow>();
        }

        public void AddTrafficFlow(ITrafficFlow trafficFlow)
        {
            _trafficFlows.Add(trafficFlow);
        }
    }
}