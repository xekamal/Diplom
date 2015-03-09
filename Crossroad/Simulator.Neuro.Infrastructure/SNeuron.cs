using Simulator.Map;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{

    public class SNeuron : ANeuron
    {
        private double _maxNofCars = 25;
        public ITrafficData TrafficData { get; set; }

        public override void Activation()
        {
            _memAxon = axon;
            if (TrafficData.TrafficDensity > _maxNofCars)
            {
                _maxNofCars = TrafficData.TrafficDensity;
            }
            axon = TrafficData.TrafficDensity/25;
        }
    }
}