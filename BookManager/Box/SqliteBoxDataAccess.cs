using Microsoft.Data.Sqlite;

namespace BookManager.Box
{
    /// <summary>
    /// SQLiteによる段ボールのデータアクセッサ
    /// </summary>
    internal class SqliteBoxDataAccess : IBoxDataAccess
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
        public SqliteBoxDataAccess(
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
        /// 段ボール全件取得
        /// </summary>
        /// <returns>段ボール一覧</returns>
        public List<BoxData> SelectAllBox()
        {
            var result = new List<BoxData>();
            var quely = """SELECT * FROM box;""";
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
                            var Box = new BoxData
                            {
                                BoxName = (string)reader["boxname"]
                            };
                            result.Add(Box);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"SelectAllBox() is Error! ({e.Message}");
                    transaction.Rollback();
                }
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 段ボールを挿入
        /// </summary>
        /// <param name="box">段ボール</param>
        public void InsertBox(BoxData box)
        {
            var quely = $"INSERT INTO box (boxname) VALUES ('{box.BoxName.Replace("'", "''")}')";
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
                    Console.Error.WriteLine($"InsertBox() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 段ボールを削除
        /// </summary>
        /// <param name="box">段ボール</param>
        public void DeleteBox(BoxData box)
        {
            var quely = $"DELETE FROM box WHERE boxname = '{box.BoxName.Replace("'", "''")}'";
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
                    Console.Error.WriteLine($"DeleteBox() is Error! ({e.Message}");
                    transaction.Rollback();
                }
            }
            connection.Close();
        }
    }
}
