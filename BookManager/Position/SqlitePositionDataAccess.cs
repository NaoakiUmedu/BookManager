using BookManager.Genre;
using Microsoft.Data.Sqlite;

namespace BookManager.Position
{
    /// <summary>
    /// SQLiteによる配置データアクセッサ
    /// </summary>
    internal class SqlitePositionDataAccess : IPositionDataAccess
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
        public SqlitePositionDataAccess(
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
        /// 配置全件取得
        /// </summary>
        /// <returns>配置一覧</returns>
        public List<PositionData> SelectAllPosition()
        {
            var result = new List<PositionData>();
            var quely = """SELECT * FROM position;""";
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
                            var position = new PositionData
                            {
                                Position = (string)reader["position"]
                            };
                            result.Add(position);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"SelectAllPosition() is Error! ({e.Message}");
                    transaction.Rollback();
                }
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 配置を挿入
        /// </summary>
        /// <param name="position">配置</param>
        public void InsertPosition(PositionData position)
        {
            var quely = $"INSERT INTO position (position) VALUES ('{position.Position.Replace("'", "''")}')";
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
                    Console.Error.WriteLine($"InsertPosition() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 配置を削除
        /// </summary>
        /// <param name="position">配置</param>
        public void DeletePosition(PositionData position)
        {
            var quely = $"DELETE FROM position WHERE position = '{position.Position.Replace("'", "''")}'";
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
                    Console.Error.WriteLine($"DeletePosition() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }
    }
}
