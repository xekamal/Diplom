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
        public void CalculateTrafficDataAddsDifferentTrafficFlows()
        {
            double density1 = 1;
            double speed1 = 10;
            var trafficFlow1 = new TrafficFlow {TrafficDensity = density1, TrafficSpeed = speed1};
            trafficFlow1.Path.Add(new Location(1, 0));
            trafficFlow1.Path.Add(new Location(1, 1));
            trafficFlow1.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(trafficFlow1);

            double density2 = 2;
            double speed2 = 12;
            var trafficFlow2 = new TrafficFlow {TrafficDensity = density2, TrafficSpeed = speed2};
            trafficFlow2.Path.Add(new Location(2, 0));
            trafficFlow2.Path.Add(new Location(1, 0));
            trafficFlow2.Path.Add(new Location(1, 1));
            trafficFlow2.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(trafficFlow2);

            _trafficManager.CalculateTrafficData();

            var crossroad = (ICrossroad) _map.GetElement(1, 1);

            Assert.AreEqual(0, crossroad.LeftToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(density1 + density2, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed1 + speed2, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);
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

        [Test]
        public void CalculateTrafficDataDoNotAddIfRedTrafficLight()
        {
            double density1 = 2;
            double speed1 = 3;
            var trafficFlow1 = new TrafficFlow {TrafficDensity = density1, TrafficSpeed = speed1};
            trafficFlow1.Path.Add(new Location(1, 0));
            trafficFlow1.Path.Add(new Location(1, 1));
            trafficFlow1.Path.Add(new Location(1, 2));
            trafficFlow1.Path.Add(new Location(1, 3));
            trafficFlow1.Path.Add(new Location(1, 4));
            _trafficManager.AddTrafficFlow(trafficFlow1);

            double density2 = 4;
            double speed2 = 7;
            var trafficFlow2 = new TrafficFlow {TrafficDensity = density2, TrafficSpeed = speed2};
            trafficFlow2.Path.Add(new Location(1, 2));
            trafficFlow2.Path.Add(new Location(1, 3));
            trafficFlow2.Path.Add(new Location(1, 4));
            _trafficManager.AddTrafficFlow(trafficFlow2);

            var crossroad1 = (ICrossroad) _map.GetElement(1, 1);
            var crossroad2 = (ICrossroad) _map.GetElement(1, 3);

            _trafficManager.CalculateTrafficData();

            Assert.AreEqual(density1, crossroad1.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed1, crossroad1.LeftToRightTrafficData.TrafficSpeed, Epsilon);

            Assert.AreEqual(density2, crossroad2.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed2, crossroad2.LeftToRightTrafficData.TrafficSpeed, Epsilon);
        }

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

        [Test]
        public void CheckAllLinesAndRecalculate()
        {
            double leftToUpDensity = 1;
            double leftToUpSpeed = 2;
            var leftToUpTrafficFlow = new TrafficFlow {TrafficDensity = leftToUpDensity, TrafficSpeed = leftToUpSpeed};
            leftToUpTrafficFlow.Path.Add(new Location(1, 0));
            leftToUpTrafficFlow.Path.Add(new Location(1, 1));
            leftToUpTrafficFlow.Path.Add(new Location(0, 1));
            _trafficManager.AddTrafficFlow(leftToUpTrafficFlow);

            double leftToRightDensity = 3;
            double leftToRightSpeed = 4;
            var leftToRightTrafficFlow = new TrafficFlow
            {
                TrafficDensity = leftToRightDensity,
                TrafficSpeed = leftToRightSpeed
            };
            leftToRightTrafficFlow.Path.Add(new Location(1, 0));
            leftToRightTrafficFlow.Path.Add(new Location(1, 1));
            leftToRightTrafficFlow.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(leftToRightTrafficFlow);

            double leftToDownDensity = 5;
            double leftToDownSpeed = 6;
            var leftToDownTrafficFlow = new TrafficFlow
            {
                TrafficDensity = leftToDownDensity,
                TrafficSpeed = leftToDownSpeed
            };
            leftToDownTrafficFlow.Path.Add(new Location(1, 0));
            leftToDownTrafficFlow.Path.Add(new Location(1, 1));
            leftToDownTrafficFlow.Path.Add(new Location(2, 1));
            _trafficManager.AddTrafficFlow(leftToDownTrafficFlow);

            double rightToDownDensity = 7;
            double rightToDownSpeed = 8;
            var rightToDownTrafficFlow = new TrafficFlow
            {
                TrafficDensity = rightToDownDensity,
                TrafficSpeed = rightToDownSpeed
            };
            rightToDownTrafficFlow.Path.Add(new Location(1, 2));
            rightToDownTrafficFlow.Path.Add(new Location(1, 1));
            rightToDownTrafficFlow.Path.Add(new Location(2, 1));
            _trafficManager.AddTrafficFlow(rightToDownTrafficFlow);

            double rightToLeftDensity = 9;
            double rightToLeftSpeed = 10;
            var rightToLeftTrafficFlow = new TrafficFlow
            {
                TrafficDensity = rightToLeftDensity,
                TrafficSpeed = rightToLeftSpeed
            };
            rightToLeftTrafficFlow.Path.Add(new Location(1, 2));
            rightToLeftTrafficFlow.Path.Add(new Location(1, 1));
            rightToLeftTrafficFlow.Path.Add(new Location(1, 0));
            _trafficManager.AddTrafficFlow(rightToLeftTrafficFlow);

            double rightToUpDensity = 11;
            double rightToUpSpeed = 12;
            var rightToUpTrafficFlow = new TrafficFlow
            {
                TrafficDensity = rightToUpDensity,
                TrafficSpeed = rightToUpSpeed
            };
            rightToUpTrafficFlow.Path.Add(new Location(1, 2));
            rightToUpTrafficFlow.Path.Add(new Location(1, 1));
            rightToUpTrafficFlow.Path.Add(new Location(0, 1));
            _trafficManager.AddTrafficFlow(rightToUpTrafficFlow);

            double upToLeftDensity = 13;
            double upToLeftSpeed = 14;
            var upToLeftTrafficFlow = new TrafficFlow {TrafficDensity = upToLeftDensity, TrafficSpeed = upToLeftSpeed};
            upToLeftTrafficFlow.Path.Add(new Location(0, 1));
            upToLeftTrafficFlow.Path.Add(new Location(1, 1));
            upToLeftTrafficFlow.Path.Add(new Location(1, 0));
            _trafficManager.AddTrafficFlow(upToLeftTrafficFlow);

            double upToDownDensity = 15;
            double upToDownSpeed = 16;
            var upToDownTrafficFlow = new TrafficFlow {TrafficDensity = upToDownDensity, TrafficSpeed = upToDownSpeed};
            upToDownTrafficFlow.Path.Add(new Location(0, 1));
            upToDownTrafficFlow.Path.Add(new Location(1, 1));
            upToDownTrafficFlow.Path.Add(new Location(2, 1));
            _trafficManager.AddTrafficFlow(upToDownTrafficFlow);

            double upToRightDensity = 17;
            double upToRightSpeed = 18;
            var upToRightTrafficFlow = new TrafficFlow
            {
                TrafficDensity = upToRightDensity,
                TrafficSpeed = upToRightSpeed
            };
            upToRightTrafficFlow.Path.Add(new Location(0, 1));
            upToRightTrafficFlow.Path.Add(new Location(1, 1));
            upToRightTrafficFlow.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(upToRightTrafficFlow);

            double downToLeftDensity = 19;
            double downToLeftSpeed = 20;
            var downToLeftTrafficFlow = new TrafficFlow
            {
                TrafficDensity = downToLeftDensity,
                TrafficSpeed = downToLeftSpeed
            };
            downToLeftTrafficFlow.Path.Add(new Location(2, 1));
            downToLeftTrafficFlow.Path.Add(new Location(1, 1));
            downToLeftTrafficFlow.Path.Add(new Location(1, 0));
            _trafficManager.AddTrafficFlow(downToLeftTrafficFlow);

            double downToUpDensity = 21;
            double downToUpSpeed = 22;
            var downToUpTrafficFlow = new TrafficFlow {TrafficDensity = downToUpDensity, TrafficSpeed = downToUpSpeed};
            downToUpTrafficFlow.Path.Add(new Location(2, 1));
            downToUpTrafficFlow.Path.Add(new Location(1, 1));
            downToUpTrafficFlow.Path.Add(new Location(0, 1));
            _trafficManager.AddTrafficFlow(downToUpTrafficFlow);

            double downToRightDensity = 23;
            double downToRightSpeed = 24;
            var downToRightTrafficFlow = new TrafficFlow
            {
                TrafficDensity = downToRightDensity,
                TrafficSpeed = downToRightSpeed
            };
            downToRightTrafficFlow.Path.Add(new Location(2, 1));
            downToRightTrafficFlow.Path.Add(new Location(1, 1));
            downToRightTrafficFlow.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(downToRightTrafficFlow);

            var crossroad = (ICrossroad) _map.GetElement(1, 1);

            _trafficManager.CalculateTrafficData();

            Assert.AreEqual(leftToUpDensity, crossroad.LeftToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(leftToUpSpeed, crossroad.LeftToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(leftToRightDensity, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(leftToRightSpeed, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(leftToDownDensity, crossroad.LeftToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(leftToDownSpeed, crossroad.LeftToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(rightToDownDensity, crossroad.RightToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(rightToDownSpeed, crossroad.RightToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(rightToLeftDensity, crossroad.RightToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(rightToLeftSpeed, crossroad.RightToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(rightToUpDensity, crossroad.RightToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(rightToUpSpeed, crossroad.RightToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(upToLeftDensity, crossroad.UpToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(upToLeftSpeed, crossroad.UpToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(upToDownDensity, crossroad.UpToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(upToDownSpeed, crossroad.UpToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(upToRightDensity, crossroad.UpToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(upToRightSpeed, crossroad.UpToRightTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(downToLeftDensity, crossroad.DownToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(downToLeftSpeed, crossroad.DownToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(downToUpDensity, crossroad.DownToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(downToUpSpeed, crossroad.DownToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(downToRightDensity, crossroad.DownToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(downToRightSpeed, crossroad.DownToRightTrafficData.TrafficSpeed, Epsilon);

            _trafficManager.CalculateTrafficData();

            Assert.AreEqual(leftToUpDensity, crossroad.LeftToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(leftToUpSpeed, crossroad.LeftToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(leftToRightDensity, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(leftToRightSpeed, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(leftToDownDensity, crossroad.LeftToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(leftToDownSpeed, crossroad.LeftToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(rightToDownDensity, crossroad.RightToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(rightToDownSpeed, crossroad.RightToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(rightToLeftDensity, crossroad.RightToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(rightToLeftSpeed, crossroad.RightToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(rightToUpDensity, crossroad.RightToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(rightToUpSpeed, crossroad.RightToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(upToLeftDensity, crossroad.UpToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(upToLeftSpeed, crossroad.UpToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(upToDownDensity, crossroad.UpToDownTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(upToDownSpeed, crossroad.UpToDownTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(upToRightDensity, crossroad.UpToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(upToRightSpeed, crossroad.UpToRightTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(downToLeftDensity, crossroad.DownToLeftTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(downToLeftSpeed, crossroad.DownToLeftTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(downToUpDensity, crossroad.DownToUpTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(downToUpSpeed, crossroad.DownToUpTrafficData.TrafficSpeed, Epsilon);
            Assert.AreEqual(downToRightDensity, crossroad.DownToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(downToRightSpeed, crossroad.DownToRightTrafficData.TrafficSpeed, Epsilon);
        }

        [Test]
        public void RecalculateTrafficDataTest()
        {
            double density = 1;
            double speed = 10;
            var trafficFlow = new TrafficFlow {TrafficDensity = density, TrafficSpeed = speed};
            trafficFlow.Path.Add(new Location(1, 0));
            trafficFlow.Path.Add(new Location(1, 1));
            trafficFlow.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(trafficFlow);
            var crossroad = (ICrossroad) _map.GetElement(1, 1);

            _trafficManager.CalculateTrafficData();

            Assert.AreEqual(density, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);

            _trafficManager.CalculateTrafficData();

            Assert.AreEqual(density, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);
        }

        [Test]
        public void RedGreenSwitchTest()
        {
            double density = 1;
            double speed = 10;
            var trafficFlow = new TrafficFlow {TrafficDensity = density, TrafficSpeed = speed};
            trafficFlow.Path.Add(new Location(1, 0));
            trafficFlow.Path.Add(new Location(1, 1));
            trafficFlow.Path.Add(new Location(1, 2));
            trafficFlow.Path.Add(new Location(1, 3));
            trafficFlow.Path.Add(new Location(1, 4));
            _trafficManager.AddTrafficFlow(trafficFlow);

            _trafficManager.CalculateTrafficData();

            var crossroad = (ICrossroad) _map.GetElement(1, 3);

            Assert.AreEqual(0, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);

            var crossroadToSwitch = (ICrossroad) _map.GetElement(1, 1);
            crossroadToSwitch.LeftToRightTrafficLight.State = TrafficLightState.Green;

            _trafficManager.CalculateTrafficData();

            Assert.AreEqual(density, crossroad.LeftToRightTrafficData.TrafficDensity, Epsilon);
            Assert.AreEqual(speed, crossroad.LeftToRightTrafficData.TrafficSpeed, Epsilon);
        }

        [Test]
        public void CalculateNofPassedCarsTest()
        {
            double density = 1;
            double speed = 10;
            var trafficFlow = new TrafficFlow { TrafficDensity = density, TrafficSpeed = speed };
            trafficFlow.Path.Add(new Location(1, 0));
            trafficFlow.Path.Add(new Location(1, 1));
            trafficFlow.Path.Add(new Location(1, 2));
            _trafficManager.AddTrafficFlow(trafficFlow);

            var crossroad = (ICrossroad)_map.GetElement(1, 1);
            crossroad.LeftToRightTrafficLight.State = TrafficLightState.Green;

            _trafficManager.CalculateTrafficData();
            _trafficManager.CalculateNofPassedCars(60);

            Assert.AreEqual(41.6666, crossroad.LeftToRightNofPassedCars, Epsilon);
        }
    }
}