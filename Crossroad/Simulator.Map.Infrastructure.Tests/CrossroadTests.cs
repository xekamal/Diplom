using NUnit.Framework;

namespace Simulator.Map.Infrastructure.Tests
{
    [TestFixture]
    public class CrossroadTests
    {
        private const double Epsilon = 0.0001;

        [Test]
        public void NofCarsIsZeroAfterCreation()
        {
            var crossroad = new Crossroad();
            Assert.AreEqual(0, crossroad.LeftToUpNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToRightNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.LeftToDownNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.UpToLeftNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.UpToDownNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.UpToRightNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.RightToUpNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.RightToLeftNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.RightToDownNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.DownToLeftNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.DownToUpNofPassedCars, Epsilon);
            Assert.AreEqual(0, crossroad.DownToRightNofPassedCars, Epsilon);
        }
    }
}