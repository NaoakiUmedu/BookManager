using Microsoft.Data.Sqlite;
using BookManager.Book;
using BookManagerTest.TestDb;
using System.Linq;

namespace BookManagerTest.Book
{
    [TestClass]
    public class SqliteBookDataAccess_Test
    {
        [TestMethod]
        public void InsertAndSelectTest()
        {
            TestDbTableCreateDelete.CreateTable();
            InitReferensedTable();

            // Arrange
            var dataAccess = new SqliteBookDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            var book = new BookData()
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
            var books = dataAccess.SelectAllBook();
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

        [TestMethod]
        public void DeleteBook_Test()
        {
            TestDbTableCreateDelete.CreateTable();
            InitReferensedTable();

            // Arrange
            var dataAccess = new SqliteBookDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            var book = new BookData()
            {
                Id = Guid.NewGuid(),
                BookName = "ある明治人の記録",
                Auther = "柴五郎",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "新書1"
            };
            dataAccess.InsertBook(book);

            // Act
            dataAccess.DeleteBook(book);

            // Assert
            var books = dataAccess.SelectAllBook();
            Assert.AreEqual(0, books.Count, $"books.Count={books.Count}");

            TestDbTableCreateDelete.DropTable();
        }

        [TestMethod]
        public void UpdateBook_Test()
        {
            TestDbTableCreateDelete.CreateTable();
            InitReferensedTable();

            // Arrange
            var dataAccess = new SqliteBookDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            var book = new BookData()
            {
                BookName = "ある明治人の記録",
                Auther = "柴五郎",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "新書1"
            };
            dataAccess.InsertBook(book);

            var updatedBook = new BookData()
            {
                Id = book.Id,
                BookName = "ある明治人の記録",
                Auther = "柴五郎",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "所属段ボール"
            };

            // Act
            dataAccess.UpdateBook(updatedBook);

            // Assert
            var books = dataAccess.SelectAllBook();
            Assert.AreEqual(1, books.Count);
            foreach (var selectedBook in books)
            {
                Assert.AreEqual(updatedBook.Id, selectedBook.Id);
                Assert.AreEqual(updatedBook.BookName, selectedBook.BookName);
                Assert.AreEqual(updatedBook.Auther, selectedBook.Auther);
                Assert.AreEqual(updatedBook.Genre, selectedBook.Genre);
                Assert.AreEqual(updatedBook.Position, selectedBook.Position);
                Assert.AreEqual(updatedBook.Box, selectedBook.Box);
            }

            TestDbTableCreateDelete.DropTable();
        }


        /// <summary>
        /// テストデータを参照してるテーブルに入れる
        /// </summary>
        private void InitReferensedTable()
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
                var boxInsert2 = """INSERT INTO box (boxname) VALUES ('所属段ボール')""";
                using (var command = new SqliteCommand(boxInsert2, connection))
                {
                    command.ExecuteNonQuery();
                }


                connection.Close();
            }
        }


    }
}
