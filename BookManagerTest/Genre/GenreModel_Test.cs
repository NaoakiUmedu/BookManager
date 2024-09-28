using BookManager.Genre;
using Moq;

namespace BookManagerTest.Genre
{
    [TestClass]
    public class GenreModel_Test
    {
        [TestMethod]
        public void InsertAndRead_Test()
        {
            // Arrange
            var testData = new List<GenreData>() {
                new GenreData(){GenreName = "歴史"},
                new GenreData(){GenreName = "小説"},
                new GenreData(){GenreName = "音楽"},
            };
            var mock = new Mock<IGenreDataAccess>();
            mock.Setup(x => x.SelectAllGenre()).Returns(testData);
            var model = new GenreUsecaseImpl(mock.Object);

            // Act
            model.Insert(testData);
            var readed = model.Read();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            readed.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(testData[i].GenreName, readed[i].GenreName);
            }
        }

        [TestMethod]
        public void Delete_Test()
        {
            // Arrange
            var testData = new List<GenreData>() {
                new GenreData(){GenreName = "歴史"},
                new GenreData(){GenreName = "小説"},
                new GenreData(){GenreName = "音楽"},
            };
            var willReturnData = new List<GenreData>() {
                new GenreData(){GenreName = "歴史"},
                new GenreData(){GenreName = "音楽"},
            };
            var mock = new Mock<IGenreDataAccess>();
            mock.Setup(x => x.SelectAllGenre()).Returns(willReturnData);
            var model = new GenreUsecaseImpl(mock.Object);

            // Act
            model.Insert(testData);
            model.Delete(new List<GenreData>() { new GenreData() { GenreName = "小説" } });
            var readed = model.Read();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            willReturnData.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(willReturnData[i].GenreName, readed[i].GenreName);
            }
        }
    }
}
