using BookManager.Book;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace BookManager.Book
{
    /// <summary>
    /// SQLiteによる蔵書データアクセッサ
    /// </summary>
    internal class SqliteBookDataAccess : IBookDataAccess
    {
        /// <summary>
        /// DBファイルのパス
        /// </summary>
        private string dbFilePath = @"Data Source=C:\MyProgramFiles\BookManager\DB\db.db";

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
        public List<BookData> SelectAllBook()
        {
            var result = new List<BookData>();
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
                            var book = new BookData();
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
        public void InsertBook(BookData book)
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
        private string GenerateInsertQuery(BookData book)
        {
            var query = string.Empty;
            query += $"INSERT INTO book";
            query += " ";
            query += "(bookid, bookname, author, genre, position, box)";
            query += " ";
            query += "VALUES";
            query += $"('{book.Id.ToString()}', '{book.BookName.Replace("'", "''")}', '{book.Auther.Replace("'", "''")}', '{book.Genre.Replace("'", "''")}', '{book.Position.Replace("'", "''")}', '{book.Box.Replace("'", "''")}')";
            return query;
        }

        /// <summary>
        /// 蔵書1冊を削除
        /// </summary>
        /// <param name="book">蔵書</param>
        public void DeleteBook(BookData book)
        {
            var quely = GenerataDeleteQuery(book);
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
        /// DELETE文を発行
        /// </summary>
        /// <param name="book">本</param>
        /// <returns>DELETE文</returns>
        private string GenerataDeleteQuery(BookData book)
        {
            var query = string.Empty;
            query += $"DELETE FROM book WHERE bookid = '{book.Id}';";
            return query;
        }

        /// <summary>
        /// 蔵書1冊を更新
        /// </summary>
        /// <param name="book">蔵書</param>
        public void UpdateBook(BookData book)
        {
            var quely = GenerateUpdateQuery(book);
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
        /// UPDATE文を発行
        /// </summary>
        /// <param name="book">蔵書</param>
        /// <returns>UPDATE文</returns>
        private string GenerateUpdateQuery(BookData book)
        {
            var query = string.Empty;
            query += "UPDATE book";
            query += " ";
            query += "SET";
            query += $"   bookid = '{book.Id}',";
            query += $"   bookname = '{book.BookName.Replace("'", "''")}',";
            query += $"   author = '{book.Auther.Replace("'", "''")}',";
            query += $"   genre = '{book.Genre.Replace("'", "''")}',";
            query += $"   position = '{book.Position.Replace("'", "''")}',";
            query += $"   box = '{book.Box.Replace("'", "''")}'";
            query += " ";
            query += $"WHERE bookid = '{book.Id}';";
            return query;
        }
}
}
