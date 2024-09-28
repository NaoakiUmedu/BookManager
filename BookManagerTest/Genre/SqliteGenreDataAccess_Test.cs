using BookManager.Genre;
using BookManagerTest.TestDb;

namespace BookManagerTest.Genre
{
    [TestClass]
    public class SqliteGenreDataAccess_Test
    {
        [TestMethod]
        public void SelectAndInsert_Test()
        {
            TestDbTableCreateDelete.CreateTable();

            // Arrange
            var testData = new List<GenreData>() { 
                new GenreData(){GenreName = "歴史"},
                new GenreData(){GenreName = "小説"},
                new GenreData(){GenreName = "音楽"},
            };

            // Act
            var da = new SqliteGenreDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            foreach(var data in testData)
            {
                da.InsertGenre(data);
            }
            var selected = da.SelectAllGenre();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            selected.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            for(var i = 0; i < selected.Count; i++)
            {
                Assert.AreEqual(testData[i].GenreName, selected[i].GenreName);
            }

            TestDbTableCreateDelete.DropTable();
        }

        [TestMethod]
        public void Delete_Test()
        {
            TestDbTableCreateDelete.CreateTable();

            // Arrange
            var testData = new List<GenreData>() {
                new GenreData(){GenreName = "歴史"},
                new GenreData(){GenreName = "小説'"},
                new GenreData(){GenreName = "音楽"},
            };
            var da = new SqliteGenreDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            foreach (var data in testData)
            {
                da.InsertGenre(data);
            }

            // Act
            da.DeleteGenre(new GenreData() { GenreName = "小説'" });
            var selected = da.SelectAllGenre();

            // Assert
            var expectData = new List<GenreData>() {
                new GenreData(){GenreName = "歴史"},
                new GenreData(){GenreName = "音楽"},
            };
            // 比較を簡略化するのためソート
            expectData.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            selected.Sort((s, t) => s.GenreName.CompareTo(t.GenreName));
            for (var i = 0; i < selected.Count; i++)
            {
                Assert.AreEqual(expectData[i].GenreName, selected[i].GenreName);
            }

            TestDbTableCreateDelete.DropTable();
        }
    }
}
