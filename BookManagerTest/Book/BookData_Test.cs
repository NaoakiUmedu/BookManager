using BookManager.Book;

namespace BookManagerTest.Book
{
    [TestClass]
    public class BookData_Test
    {
        [TestMethod]
        public void Equal_Test()
        {
            var sameId = Guid.NewGuid();
            var l = new BookData() { Id = sameId };
            var r = new BookData() { Id = sameId };
            Assert.IsTrue(l.Equals(r));
            Assert.IsTrue(r.Equals(l));
            Assert.IsTrue(l.Equals((object?)r));
        }

        [TestMethod]
        public void NotEqual_Test()
        {
            var l = new BookData() { };
            var r = new BookData() { };
            Assert.IsFalse(l.Equals(r));
            Assert.IsFalse(r.Equals(l));
            Assert.IsFalse(l.Equals((object?)r));
            Assert.IsFalse(l.Equals(null));
            Assert.IsFalse(r.Equals(null));
        }

        [TestMethod]
        public void GetHashCode_Test()
        {
            var l = new BookData() { };
            var r = new BookData() { };
            Assert.AreNotEqual(l.GetHashCode(), r.GetHashCode());
        }
    }
}
