using BookManager.Box;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerTest.Box
{
    [TestClass]
    public class BoxData_Test
    {
        [TestMethod]
        public void Equal_Test()
        {
            var l = new BoxData() { BoxName = "文庫1(エンタメ)" };
            var r = new BoxData() { BoxName = "文庫1(エンタメ)" };
            Assert.IsTrue(l.Equals(r));
            Assert.IsTrue(r.Equals(l));
            Assert.IsTrue(l.Equals((object?)r));
        }

        [TestMethod]
        public void NotEqual_Test()
        {
            var l = new BoxData() { BoxName = "文庫1(エンタメ)" };
            var r = new BoxData() { BoxName = "文庫2(教養)" };
            Assert.IsFalse(l.Equals(r));
            Assert.IsFalse(r.Equals(l));
            Assert.IsFalse(l.Equals((object?)r));
            Assert.IsFalse(l.Equals(null));
            Assert.IsFalse(r.Equals(null));
        }
    }
}
