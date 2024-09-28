using BookManager.Position;
using Moq;

namespace BookManagerTest.Position
{
    [TestClass]
    public class PositionModel_Test
    {
        [TestMethod]
        public void InsertAndRead_Test()
        {
            // Arrange
            var testData = new List<PositionData>() {
                new PositionData(){Position = "本棚(小)"},
                new PositionData(){Position = "所属段ボール"},
                new PositionData(){Position = "本棚(大)"},
            };
            var mock = new Mock<IPositionDataAccess>();
            mock.Setup(x => x.SelectAllPosition()).Returns(testData);
            var model = new PositionUsecaseImpl(mock.Object);

            // Act
            model.Insert(testData);
            var readed = model.Read();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.Position.CompareTo(t.Position));
            readed.Sort((s, t) => s.Position.CompareTo(t.Position));
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(testData[i].Position, readed[i].Position);
            }
        }

        [TestMethod]
        public void Delete_Test()
        {
            // Arrange
            var testData = new List<PositionData>() {
                new PositionData(){Position = "本棚(小)"},
                new PositionData(){Position = "所属段ボール"},
                new PositionData(){Position = "本棚(大)"},
            };
            var willReturnData = new List<PositionData>() {
                new PositionData(){Position = "本棚(小)"},
                new PositionData(){Position = "本棚(大)"},
            };
            var mock = new Mock<IPositionDataAccess>();
            mock.Setup(x => x.SelectAllPosition()).Returns(willReturnData);
            var model = new PositionUsecaseImpl(mock.Object);

            // Act
            model.Insert(testData);
            model.Delete(new List<PositionData>() { new PositionData() { Position = "所属段ボール" } });
            var readed = model.Read();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.Position.CompareTo(t.Position));
            willReturnData.Sort((s, t) => s.Position.CompareTo(t.Position));
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(willReturnData[i].Position, readed[i].Position);
            }
        }
    }
}
