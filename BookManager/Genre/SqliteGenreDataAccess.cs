using BookManager.Box;
using Microsoft.Data.Sqlite;

namespace BookManager.Genre
{
    /// <summary>
    /// SQLiteによるジャンルデータアクセッサ
    /// </summary>
    internal class SqliteGenreDataAccess : IGenreDataAccess
    {
        /// <summary>
        /// DBファイルのパス(デフォルトは本番DB)
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
        public SqliteGenreDataAccess(
            string? connectionString = null,
            System.Data.IsolationLevel? isolationLevel = null)
        {
            if (connectionString != null)
            {
                this.dbFilePath = connectionString;
            }
            if (isolationLevel != null)
            {
                this.isolationLevel = (System.Data.IsolationLevel)isolationLevel;
            }
        }

        /// <summary>
        /// ジャンル全件取得
        /// </summary>
        /// <returns>ジャンル一覧</returns>
        public List<GenreData> SelectAllGenre()
        {
            var result = new List<GenreData>();
            var quely = """SELECT * FROM genre;""";
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
                            var genre = new GenreData
                            {
                                GenreName = (string)reader["genrename"]
                            };
                            result.Add(genre);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"SelectAllGenre() is Error! ({e.Message}");
                    transaction.Rollback();
                }
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void InsertGenre(GenreData genre)
        {
            var quely = $"INSERT INTO genre (genrename) VALUES ('{genre.GenreName.Replace("'", "''")}')";
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
                    Console.Error.WriteLine($"InsertGenre() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }

        /// <summary>
        /// ジャンルを削除
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void DeleteGenre(GenreData genre)
        {
            var quely = $"DELETE FROM genre WHERE genrename = '{genre.GenreName.Replace("'", "''")}'";
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
                    Console.Error.WriteLine($"DeleteGenre() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }
    }
}
