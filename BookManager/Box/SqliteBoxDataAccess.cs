using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string dbFilePath = @"Data Source=C:\MyProgramFiles\BookManager\DB\db.db";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">DBファイルのパス(省略で本番ファイル)</param>
        public SqliteBoxDataAccess(string? connectionString = null)
        {
            if (connectionString != null)
            {
                this.dbFilePath = connectionString;
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
                using (var command = new SqliteCommand(quely, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Box = new BoxData();
                            Box.BoxName = (string)reader["boxname"];
                            result.Add(Box);
                        }
                    }
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
        /// 段ボールを削除
        /// </summary>
        /// <param name="box">段ボール</param>
        public void DeleteBox(BoxData box)
        {
            var quely = $"DELETE FROM box WHERE boxname = '{box.BoxName.Replace("'", "''")}'";
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
    }
}
