using Microsoft.Data.Sqlite;
using BookManager.Book;
using BookManagerTest.TestDb;

namespace BookManagerTest.Book
{
    [TestClass]
    public class SqliteBookDataAccess_Test
    {
        [TestMethod]
        public void InsertAndSelectTest()
        {
            TestDbTableCreateDelete.CreateTable();
            InitTable();

            // Arrange
            var dataAccess = new SqliteBookDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            var book = new BookModel.BookData()
            {
                BookName = "ある明治人の記録",
                Auther = "柴五郎",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "新書1"
            };

            // Act
            dataAccess.InsertBook(book);

            // Assert
            var books = dataAccess.SelectAllBooks();
            foreach (var selectedBook in books)
            {
                Assert.AreEqual(book.Id, selectedBook.Id);
                Assert.AreEqual(book.BookName, selectedBook.BookName);
                Assert.AreEqual(book.Auther, selectedBook.Auther);
                Assert.AreEqual(book.Genre, selectedBook.Genre);
                Assert.AreEqual(book.Position, selectedBook.Position);
                Assert.AreEqual(book.Box, selectedBook.Box);
            }

            TestDbTableCreateDelete.DropTable();
        }


        /// <summary>
        /// テストデータをテーブルに入れる
        /// </summary>
        private void InitTable()
        {
            using (var connection = new SqliteConnection(TestDbTableCreateDelete.TEST_DB_FILE_PATH))
            {
                connection.Open();

                var genreInsert = """INSERT INTO genre (genrename) VALUES ('歴史');""";
                using (var command = new SqliteCommand(genreInsert, connection))
                {
                    command.ExecuteNonQuery();
                }

                var positionInsert = """INSERT INTO position (position) VALUES ('本棚(小)');""";
                using (var command = new SqliteCommand(positionInsert, connection))
                {
                    command.ExecuteNonQuery();
                }

                var boxInsert = """INSERT INTO box (boxname) VALUES ('新書1')""";
                using (var command = new SqliteCommand(boxInsert, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }


    }
}
