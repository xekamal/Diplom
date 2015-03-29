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
            engine.Step(60);

            Assert.AreEqual(TrafficLightState.Green, crossroad.LeftToRightTrafficLight.State);

        }

        [Test]
        public void step60OnceMore()
        {
            /* Initialize next map
             * 
             *    | | |
             *   -+-+-+-
             *   || | ||
             *   -+-+-+-
             *    | | |
             */

            var map = new Map.Infrastructure.Map(5, 7);

            map.AddElement(0, 1, new Road());
            map.AddElement(0, 3, new Road());
            map.AddElement(0, 5, new Road());

            map.AddElement(1, 0, new Turn());
            map.AddElement(1, 1, new Crossroad());
            map.AddElement(1, 2, new Road());
            map.AddElement(1, 3, new Crossroad());
            map.AddElement(1, 4, new Road());
            map.AddElement(1, 5, new Crossroad());
            map.AddElement(1, 6, new Turn());

            map.AddElement(2, 0, new Road());
            map.AddElement(2, 1, new Road());
            map.AddElement(2, 3, new Road());
            map.AddElement(2, 5, new Road());
            map.AddElement(2, 6, new Road());

            map.AddElement(3, 0, new Turn());
            map.AddElement(3, 1, new Crossroad());
            map.AddElement(3, 2, new Road());
            map.AddElement(3, 3, new Crossroad());
            map.AddElement(3, 4, new Road());
            map.AddElement(3, 5, new Crossroad());
            map.AddElement(3, 6, new Turn());

            map.AddElement(4, 1, new Road());
            map.AddElement(4, 3, new Road());
            map.AddElement(4, 5, new Road());

            map.SetConnected(0, 1, 1, 1);
            map.SetConnected(0, 3, 1, 3);
            map.SetConnected(0, 5, 1, 5);

            map.SetConnected(1, 0, 1, 1);
            map.SetConnected(1, 0, 2, 0);
            map.SetConnected(1, 1, 1, 0);
            map.SetConnected(1, 1, 2, 1);
            map.SetConnected(1, 1, 1, 2);
            map.SetConnected(1, 2, 1, 3);
            map.SetConnected(1, 3, 2, 3);
            map.SetConnected(1, 3, 1, 4);
            map.SetConnected(1, 4, 1, 5);
            map.SetConnected(1, 5, 2, 5);
            map.SetConnected(1, 5, 1, 6);
            map.SetConnected(1, 6, 2, 6);

            map.SetConnected(2, 0, 3, 0);
            map.SetConnected(2, 1, 3, 1);
            map.SetConnected(2, 3, 3, 3);
            map.SetConnected(2, 3, 3, 3);
            map.SetConnected(2, 5, 3, 5);
            map.SetConnected(2, 6, 3, 6);

            map.SetConnected(3, 0, 3, 1);
            map.SetConnected(3, 1, 4, 1);
            map.SetConnected(3, 1, 3, 2);
            map.SetConnected(3, 2, 3, 3);
            map.SetConnected(3, 3, 4, 3);
            map.SetConnected(3, 3, 3, 4);
            map.SetConnected(3, 4, 3, 5);
            map.SetConnected(3, 5, 4, 5);
            map.SetConnected(3, 5, 3, 6);

            var trafficFlow = new TrafficFlow();
            trafficFlow.TrafficDensity = 0.1;
            trafficFlow.TrafficSpeed = 60;
            trafficFlow.Path.Add(new Location(0, 1));
            trafficFlow.Path.Add(new Location(1, 1));
            trafficFlow.Path.Add(new Location(2, 1));
            trafficFlow.Path.Add(new Location(3, 1));
            trafficFlow.Path.Add(new Location(3, 2));
            trafficFlow.Path.Add(new Location(3, 3));
            trafficFlow.Path.Add(new Location(3, 4));

            var trafficFlow1 = new TrafficFlow();
            trafficFlow1.TrafficDensity = 0.6;
            trafficFlow1.TrafficSpeed = 30;
            trafficFlow1.Path.Add(new Location(0, 1));
            trafficFlow1.Path.Add(new Location(1, 1));
            trafficFlow1.Path.Add(new Location(1, 2));
            trafficFlow1.Path.Add(new Location(1, 3));
            trafficFlow1.Path.Add(new Location(2, 3));
            trafficFlow1.Path.Add(new Location(3, 3));
            trafficFlow1.Path.Add(new Location(3, 4));

            var trafficManager = new TrafficManager(map);
            trafficManager.AddTrafficFlow(trafficFlow);
            trafficManager.AddTrafficFlow(trafficFlow1);
            
            var engine = new SimulatorEngine(trafficManager);

            var crossroad = (ICrossroad)map.GetElement(1, 1);
            Assert.AreEqual(crossroad.UpToDownTrafficLight.State, TrafficLightState.Red);
            Assert.AreEqual(crossroad.UpToRightTrafficLight.State, TrafficLightState.Red);
            engine.Step(60);
            engine.Step(60);
            engine.Step(60);
            engine.Step(60);
            
            Assert.AreEqual(TrafficLightState.Green, crossroad.UpToDownTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green, crossroad.UpToRightTrafficLight.State);
            
            engine.Step(60);
            Assert.AreEqual(TrafficLightState.Green, crossroad.UpToDownTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green, crossroad.UpToRightTrafficLight.State);

        }
    }
}