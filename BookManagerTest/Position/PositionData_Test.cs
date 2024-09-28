using BookManager.Position;

namespace BookManagerTest.Position
{
    [TestClass]
    public class PositionData_Test
    {
        [TestMethod]
        public void Equal_Test()
        {
            var l = new PositionData() { Position = "本棚(大)" };
            var r = new PositionData() { Position = "本棚(大)" };
            Assert.IsTrue(l.Equals(r));
            Assert.IsTrue(r.Equals(l));
            Assert.IsTrue(l.Equals((object?)r));
        }

        [TestMethod]
        public void NotEqual_Test()
        {
            var l = new PositionData() { Position = "本棚(大)" };
            var r = new PositionData() { Position = "本棚(小)" };
            Assert.IsFalse(l.Equals(r));
            Assert.IsFalse(r.Equals(l));
            Assert.IsFalse(l.Equals((object?)r));
            Assert.IsFalse(l.Equals(null));
            Assert.IsFalse(r.Equals(null));
        }
    }
}
