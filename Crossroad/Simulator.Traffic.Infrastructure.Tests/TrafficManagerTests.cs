using NUnit.Framework;
using Simulator.Map;
using Simulator.Map.Infrastructure;
using Simulator.Traffic.Domain;

namespace Simulator.Traffic.Infrastructure.Tests
{
    [TestFixture]
    public class TrafficManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            /* Initialize next map
             * 
             *    | | |
             *   -+-+-+-
             *   || | ||
             *   -+-+-+-
             *    | | |
             */

            _map = new Map.Infrastructure.Map(5, 7);

            _map.AddElement(0, 1, new Road());
            _map.AddElement(0, 3, new Road());
            _map.AddElement(0, 5, new Road());

            _map.AddElement(1, 0, new Turn());
            _map.AddElement(1, 1, new Crossroad());
            _map.AddElement(1, 2, new Road());
            _map.AddElement(1, 3, new Crossroad());
            _map.AddElement(1, 4, new Road());
            _map.AddElement(1, 5, new Crossroad());
            _map.AddElement(1, 6, new Turn());

            _map.AddElement(2, 0, new Road());
            _map.AddElement(2, 1, new Road());
            _map.AddElement(2, 3, new Road());
            _map.AddElement(2, 5, new Road());
            _map.AddElement(2, 6, new Road());

            _map.AddElement(3, 0, new Turn());
            _map.AddElement(3, 1, new Crossroad());
            _map.AddElement(3, 2, new Road());
            _map.AddElement(3, 3, new Crossroad());
            _map.AddElement(3, 4, new Road());
            _map.AddElement(3, 5, new Crossroad());
            _map.AddElement(3, 6, new Turn());

            _map.AddElement(4, 1, new Road());
            _map.AddElement(4, 3, new Road());
            _map.AddElement(4, 5, new Road());

            _map.SetConnected(0, 1, 1, 1);
            _map.SetConnected(0, 3, 1, 3);
            _map.SetConnected(0, 5, 1, 5);

            _map.SetConnected(1, 0, 1, 1);
            _map.SetConnected(1, 0, 2, 0);
            _map.SetConnected(1, 1, 1, 0);
            _map.SetConnected(1, 1, 2, 1);
            _map.SetConnected(1, 1, 1, 2);
            _map.SetConnected(1, 2, 1, 3);
            _map.SetConnected(1, 3, 2, 3);
            _map.SetConnected(1, 3, 1, 4);
            _map.SetConnected(1, 4, 1, 5);
            _map.SetConnected(1, 5, 2, 5);
            _map.SetConnected(1, 5, 1, 6);
            _map.SetConnected(1, 6, 2, 6);

            _map.SetConnected(2, 0, 3, 0);
            _map.SetConnected(2, 1, 3, 1);
            _map.SetConnected(2, 3, 3, 3);
            _map.SetConnected(2, 3, 3, 3);
            _map.SetConnected(2, 5, 3, 5);
            _map.SetConnected(2, 6, 3, 6);

            _map.SetConnected(3, 0, 3, 1);
            _map.SetConnected(3, 1, 4, 1);
            _map.SetConnected(3, 1, 3, 2);
            _map.SetConnected(3, 2, 3, 3);
            _map.SetConnected(3, 3, 4, 3);
            _map.SetConnected(3, 3, 3, 4);
            _map.SetConnected(3, 4, 3, 5);
            _map.SetConnected(3, 5, 4, 5);
            _map.SetConnected(3, 5, 3, 6);

            _trafficManager = new TrafficManager(_map);
        }

        private const double Epsilon = 0.0001;

        private IMap _map;
        private ITrafficManager _trafficManager;

        [Test]
        public void CalculateTrafficDataSetsInputTrafficParameters()
        {
            double density = 1;
            double speed = 10;
            var trafficFlow = new TrafficFlow {TrafficDensity = density, TrafficSpeed = speed};
            trafficFlow.Path.Add(new Location(1, 0));
            trafficFlow.Path.Add(new Location(1, 1));
            trafficFlow.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(trafficFlow);

            _trafficManager.CalculateTrafficData();

            var crossroad = (ICrossroad) _map.GetElement(1, 1);

            Assert.AreEqual(0, crossroad.LeftToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(density, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToDownTrafficData.TrafficSpeed, Epsilon);

            Assert.AreEqual(0, crossroad.UpToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.UpToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.UpToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.UpToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.UpToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.UpToRightTrafficData.TrafficSpeed, Epsilon);

            Assert.AreEqual(0, crossroad.RightToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.RightToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.RightToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.RightToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.RightToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.RightToDownTrafficData.TrafficSpeed, Epsilon);

            Assert.AreEqual(0, crossroad.DownToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.DownToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.DownToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.DownToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(0, crossroad.DownToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.DownToRightTrafficData.TrafficSpeed, Epsilon);
        }
    }
}