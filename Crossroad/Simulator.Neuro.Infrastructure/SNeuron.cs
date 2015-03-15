using System;
using Simulator.Map;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{

    public class SNeuron : ANeuron
    {
        public ITrafficData TrafficData { get; set; }

        public override void Activation()
        {
            _memAxon = axon;
            double speed = 0;
            if (Math.Abs(TrafficData.TrafficSpeed) > 0.0001 )
            {
                speed = 1 / (TrafficData.TrafficSpeed / 100);
            }
            axon = TrafficData.TrafficDensity+speed;
        }
    }
}