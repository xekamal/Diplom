using NUnit.Framework;

namespace Simulator.Map.Infrastructure.Tests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void AllElementsAreNotConnectedAfterCreation()
        {
            IMap map = new Map(2, 2);
            for (int e1Row = 0; e1Row < 2; e1Row++)
            {
                for (int e1Column = 0; e1Column < 2; e1Column++)
                {
                    for (int e2Row = 0; e2Row < 2; e2Row++)
                    {
                        for (int e2Column = 0; e2Column < 2; e2Column++)
                        {
                            Assert.False(map.IsConnected(e1Row, e1Column, e2Row, e2Column));
                        }
                    }
                }
            }
        }

        [Test]
        public void AllElementsAreNullAfterCreation()
        {
            IMap map = new Map(3, 4);
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    Assert.IsNull(map.GetElement(row, column));
                }
            }
        }

        [Test]
        public void ElementIsNotNullAfterSet()
        {
            IMap map = new Map(1, 2);
            Assert.IsNull(map.GetElement(0, 1));
            map.AddElement(0, 1, new Road());
            Assert.NotNull(map.GetElement(0, 1));
        }

        [Test]
        public void ElementsAreConnectedAfterSetConnected()
        {
            IMap map = new Map(2, 2);
            Assert.False(map.IsConnected(0, 1, 1, 0));
            Assert.False(map.IsConnected(1, 0, 0, 1));
            map.SetConnected(0, 1, 1, 0);
            Assert.True(map.IsConnected(0, 1, 1, 0));
            Assert.True(map.IsConnected(1, 0, 0, 1));
        }

        [Test]
        public void FindShortestPathTest1()
        {
            IMap map = new Map(2, 2);
            map.SetConnected(0, 0, 0, 1);
            map.SetConnected(0, 0, 1, 0);
            map.SetConnected(0, 0, 1, 1);
            map.SetConnected(0, 1, 1, 1);
            map.SetConnected(1, 0, 1, 1);

            ILocation[] path = map.FindShortestPath(new Location(0, 0), new Location(1, 1));

            Assert.AreEqual(2, path.Length);
            Assert.AreEqual(0, path[0].Row);
            Assert.AreEqual(0, path[0].Column);
            Assert.AreEqual(1, path[1].Row);
            Assert.AreEqual(1, path[1].Column);
        }

        [Test]
        public void FindShortestPathTest2()
        {
            IMap map = new Map(4, 4);
            map.SetConnected(3, 0, 2, 0);
            map.SetConnected(2, 0, 2, 1);
            map.SetConnected(2, 1, 1, 1);
            map.SetConnected(1, 1, 0, 1);
            map.SetConnected(0, 1, 0, 2);
            map.SetConnected(0, 2, 0, 3);
            map.SetConnected(3, 0, 3, 1);
            map.SetConnected(3, 1, 3, 2);
            map.SetConnected(3, 3, 3, 3);
            map.SetConnected(3, 3, 2, 3);
            map.SetConnected(2, 3, 2, 2);
            map.SetConnected(2, 2, 1, 2);
            map.SetConnected(1, 2, 1, 3);
            map.SetConnected(1, 3, 0, 3);

            ILocation[] path = map.FindShortestPath(new Location(3, 0), new Location(0, 3));

            Assert.AreEqual(7, path.Length);
        }
    }
}