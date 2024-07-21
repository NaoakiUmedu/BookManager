using BookManager.Box;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Windows.Controls.Primitives;

namespace BookManagerTest.Box
{
    [TestClass]
    public class BoxModel_Test
    {
        [TestMethod]
        public void InsertAndRead_Test()
        {
            // Arrange
            var testData = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "文庫2(教養)"},
                new BoxData(){BoxName = "漫画1"},
            };
            var mock = new Mock<IBoxDataAccess>();
            mock.Setup(x => x.SelectAllBox()).Returns(testData);
            var model = new BoxModel(mock.Object);

            // Act
            model.Insert(testData);
            var readed = model.Read();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            readed.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(testData[i].BoxName, readed[i].BoxName);
            }
        }

        [TestMethod]
        public void Delete_Test()
        {
            // Arrange
            var testData = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "文庫2(教養)"},
                new BoxData(){BoxName = "漫画1"},
            };
            var willReturnData = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "漫画1"},
            };
            var mock = new Mock<IBoxDataAccess>();
            mock.Setup(x => x.SelectAllBox()).Returns(willReturnData);
            var model = new BoxModel(mock.Object);

            // Act
            model.Insert(testData);
            model.Delete(new List<BoxData>() { new BoxData() { BoxName = "文庫2(教養)" } });
            var readed = model.Read();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            willReturnData.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(willReturnData[i].BoxName, readed[i].BoxName);
            }
        }
    }
}
