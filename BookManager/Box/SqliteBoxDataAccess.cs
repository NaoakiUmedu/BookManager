using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Box
{
    internal class SqliteBoxDataAccess : IBoxDataAccess
    {
        /// <summary>
        /// DBファイルのパス(デフォルトは本番DB)
        /// </summary>
        private string dbFilePath = @"Data Source=C:\Users\anija\Desktop\Apps\BookManager\DB\db.db";

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
            var quely = """SELECT * FROM Box;""";
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
                            Box.BoxName = (string)reader["Boxname"];
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
        /// <param name="Box">段ボール</param>
        public void InsertBox(BoxData Box)
        {
            var quely = $"INSERT INTO Box (Boxname) VALUES ('{Box.BoxName}')";
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
        /// <param name="Box">段ボール</param>
        public void DeleteBox(BoxData Box)
        {
            var quely = $"DELETE FROM Box WHERE Boxname = '{Box.BoxName}'";
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
