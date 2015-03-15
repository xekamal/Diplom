
using NUnit.Framework;
using Simulator.Map;
using Simulator.Map.Infrastructure;

namespace Simulator.Neuro.Infrastructure.Tests
{
    [TestFixture]
    public class CrossroadControllerReinforcementTests
    {
        [Test]
        public void CrossroadCreation()
        {
            ICrossroad crossroad = new Crossroad();
        }
        [Test]
        public void CheckStates()
        {
            ICrossroad crossroad = new Crossroad();
            crossroad.DownToRightTrafficData.TrafficDensity = 0.78;
            crossroad.DownToRightTrafficData.TrafficSpeed = 20;

            crossroad.LeftToRightTrafficData.TrafficDensity = 0.6;
            crossroad.LeftToRightTrafficData.TrafficSpeed = 30;

            crossroad.UpToRightTrafficData.TrafficDensity = 0.3;
            crossroad.UpToRightTrafficData.TrafficSpeed = 60;

            crossroad.CrossroadController.Step();
            Assert.AreEqual(TrafficLightState.Green,crossroad.UpToLeftTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Red,crossroad.RightToLeftTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Red,crossroad.DownToLeftTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green,crossroad.LeftToDownTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Red,crossroad.UpToDownTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Red,crossroad.RightToDownTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green,crossroad.DownToRightTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green,crossroad.LeftToRightTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green,crossroad.UpToRightTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Red,crossroad.RightToUpTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Red,crossroad.DownToUpTrafficLight.State);
            Assert.AreEqual(TrafficLightState.Green,crossroad.LeftToUpTrafficLight.State);
        }
    }
}