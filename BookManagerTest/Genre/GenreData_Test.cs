using BookManager.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerTest.Genre
{
    [TestClass]
    public class GenreData_Test
    {
        [TestMethod]
        public void Equal_Test()
        {
            var l = new GenreData() { GenreName = "歴史" };
            var r = new GenreData() { GenreName = "歴史" };
            Assert.IsTrue(l.Equals(r));
            Assert.IsTrue(r.Equals(l));
            Assert.IsTrue(l.Equals((object?)r));
        }

        [TestMethod]
        public void NotEqual_Test()
        {
            var l = new GenreData() { GenreName = "歴史" };
            var r = new GenreData() { GenreName = "小説" };
            Assert.IsFalse(l.Equals(r));
            Assert.IsFalse(r.Equals(l));
            Assert.IsFalse(l.Equals((object?)r));
            Assert.IsFalse(l.Equals(null));
            Assert.IsFalse(r.Equals(null));
        }
    }
}
