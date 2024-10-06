using Microsoft.Data.Sqlite;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        private readonly string dbFilePath = @"Data Source=C:\MyProgramFiles\BookManager\DB\db.db";

        /// <summary>
        /// アイソレーションレベル
        /// </summary>
        private readonly System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">DBファイルのパス(省略で本番ファイル)</param>
        /// <param name="isolationLevel">アイソレーションレベル(省略でSerializable)</param>
        public SqliteBookDataAccess(
            string? connectionString = null,
            System.Data.IsolationLevel? isolationLevel = null)
        {
            if(connectionString != null)
            {
                this.dbFilePath = connectionString;
            }
            if(isolationLevel != null)
            {
                this.isolationLevel = (System.Data.IsolationLevel)isolationLevel;
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
                using var transaction = connection.BeginTransaction(isolationLevel: isolationLevel);
                try
                {
                    using (var command = new SqliteCommand(quely, connection))
                    {
                        command.Transaction = transaction;
                        using var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var book = new BookData
                            {
                                Id = Guid.Parse((string)reader["bookid"]),
                                BookName = (string)reader["bookname"],
                                Auther = (string)reader["author"],
                                Genre = (string)reader["genre"],
                                Position = (string)reader["position"],
                                Box = (string)reader["box"]
                            };
                            result.Add(book);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"SelectAllBook() is Error! ({e.Message}");
                    transaction.Rollback();
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
            using var connection = new SqliteConnection(dbFilePath);
            connection.Open();
            using (SqliteTransaction transaction = connection.BeginTransaction(isolationLevel: isolationLevel))
            {
                try
                {
                    using (var command = new SqliteCommand(quely, connection))
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"InsertBook() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }

        /// <summary>
        ///INSERT文を発行
        /// </summary>
        /// <param name="book">蔵書</param>
        /// <returns>INSERT文</returns>
        /// <remarks>
        /// コード解析結果CA1822に対応 パフォーマンス上優位なためstaticとした
        /// staticとした理由はそれだけなので、必用があればstaticを外しても問題ない
        /// </remarks>
        private static string GenerateInsertQuery(BookData book)
        {
            var query = string.Empty;
            query += $"INSERT INTO book";
            query += " ";
            query += "(bookid, bookname, author, genre, position, box)";
            query += " ";
            query += "VALUES";
            query += $"('{book.Id}', '{book.BookName.Replace("'", "''")}', '{book.Auther.Replace("'", "''")}', '{book.Genre.Replace("'", "''")}', '{book.Position.Replace("'", "''")}', '{book.Box.Replace("'", "''")}')";
            return query;
        }

        /// <summary>
        /// 蔵書1冊を削除
        /// </summary>
        /// <param name="book">蔵書</param>
        public void DeleteBook(BookData book)
        {
            var quely = GenerataDeleteQuery(book);
            using var connection = new SqliteConnection(dbFilePath);
            connection.Open();
            using (SqliteTransaction transaction = connection.BeginTransaction(isolationLevel: isolationLevel))
            {
                try
                {
                    using (var command = new SqliteCommand(quely, connection))
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"DeleteBook() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }
        /// <summary>
        /// DELETE文を発行
        /// </summary>
        /// <param name="book">本</param>
        /// <returns>DELETE文</returns>
        /// <remarks>
        /// コード解析結果CA1822に対応 パフォーマンス上優位なためstaticとした
        /// staticとした理由はそれだけなので、必用があればstaticを外しても問題ない
        /// </remarks>
        private static string GenerataDeleteQuery(BookData book)
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
            using var connection = new SqliteConnection(dbFilePath);
            connection.Open();
            using (SqliteTransaction transaction = connection.BeginTransaction(isolationLevel: isolationLevel))
            {
                try
                {
                    using (var command = new SqliteCommand(quely, connection))
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"DeleteBook() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }
        /// <summary>
        /// UPDATE文を発行
        /// </summary>
        /// <param name="book">蔵書</param>
        /// <returns>UPDATE文</returns>
        /// <remarks>
        /// コード解析結果CA1822に対応 パフォーマンス上優位なためstaticとした
        /// staticとした理由はそれだけなので、必用があればstaticを外しても問題ない
        /// </remarks>
        private static string GenerateUpdateQuery(BookData book)
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
