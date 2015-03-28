using NUnit.Framework;
using Simulator.Map;
using Simulator.Map.Infrastructure;
using Simulator.Traffic.Infrastructure;

namespace Simulator.Engine.Infrastructure.Tests
{
    [TestFixture]
    public class SimulatorEngineTests
    {
        private const double Epsilon = 0.0001;

        [Test]
        public void Step60SecondsTest()
        {
            var map = new Map.Infrastructure.Map(1, 3);
            map.AddElement(0, 0, new Road());
            map.AddElement(0, 1, new Crossroad());
            map.AddElement(0, 2, new Road());
            map.SetConnected(0, 0, 0, 1);
            map.SetConnected(0, 1, 0, 2);

            var trafficFlow = new TrafficFlow();
            trafficFlow.TrafficDensity = 0.1;
            trafficFlow.TrafficSpeed = 10;
            trafficFlow.Path.Add(new Location(0, 0));
            trafficFlow.Path.Add(new Location(0, 1));
            trafficFlow.Path.Add(new Location(0, 2));

            var trafficManager = new TrafficManager(map);
            trafficManager.AddTrafficFlow(trafficFlow);

            var engine = new SimulatorEngine(trafficManager);

            var crossroad = (ICrossroad) map.GetElement(0, 1);
            Assert.AreEqual(crossroad.LeftToRightTrafficLight.State, TrafficLightState.Red);
            Assert.AreEqual(crossroad.LeftToRightTrafficData.TrafficDensity, trafficFlow.TrafficDensity, Epsilon);
            Assert.AreEqual(crossroad.LeftToRightTrafficData.TrafficSpeed, trafficFlow.TrafficSpeed, Epsilon);

            engine.Step(60);

            Assert.AreEqual(TrafficLightState.Green, crossroad.LeftToRightTrafficLight.State);
        }
    }
}