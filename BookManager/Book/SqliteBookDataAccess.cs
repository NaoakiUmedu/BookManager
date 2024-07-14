using BookManager.Book;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace BookManager.Book
{
    internal class SqliteBookDataAccess : IBookDataAccess
    {
        /// <summary>
        /// DBファイルのパス
        /// </summary>
        private string dbFilePath = @"Data Source=C:\Users\anija\Desktop\Apps\BookManager\DB\db.db";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">DBファイルのパス(省略で本番ファイル)</param>
        public SqliteBookDataAccess(string? connectionString = null)
        {
            if(connectionString != null)
            {
                this.dbFilePath = connectionString;
            }
        }

        /// <summary>
        /// 蔵書全件取得
        /// </summary>
        /// <returns>蔵書一覧</returns>
        public List<BookModel.BookData> SelectAllBooks()
        {
            var result = new List<BookModel.BookData>();
            var quely = """SELECT * FROM book;""";
            using (var connection = new SqliteConnection(dbFilePath))
            {
                connection.Open();
                using (var command = new SqliteCommand(quely, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new BookModel.BookData();
                            book.Id = Guid.Parse((string)reader["bookid"]);
                            book.BookName = (string)reader["bookname"];
                            book.Auther = (string)reader["author"];
                            book.Genre = (string)reader["genre"];
                            book.Position = (string)reader["position"];
                            book.Box = (string)reader["box"];
                            result.Add(book);
                        }
                    }
                }
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 蔵書1冊を挿入
        /// </summary>
        /// <param name="book">蔵書</param>
        public void InsertBook(BookModel.BookData book)
        {
            var quely = GenerateInsertQuery(book);
            using (var connection = new SqliteConnection(dbFilePath))
            {
                connection.Open();
                using (var command = new SqliteCommand(quely, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        ///INSERT文を発行
        /// </summary>
        /// <param name="book">蔵書</param>
        /// <returns>INSERT文</returns>
        private string GenerateInsertQuery(BookModel.BookData book)
        {
            var query = string.Empty;
            query += $"INSERT INTO book";
            query += " ";
            query += "(bookid, bookname, author, genre, position, box)";
            query += " ";
            query += "VALUES";
            query += $"('{book.Id.ToString()}', '{book.BookName}', '{book.Auther}', '{book.Genre}', '{book.Position}', '{book.Box}')";
            return query;
        }
    }
}
