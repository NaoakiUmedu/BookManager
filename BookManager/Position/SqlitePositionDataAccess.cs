using BookManager.Position;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Position
{
    internal class SqlitePositionDataAccess : IPositionDataAccess
    {
        /// <summary>
        /// DBファイルのパス(デフォルトは本番DB)
        /// </summary>
        private string dbFilePath = @"Data Source=C:\Users\anija\Desktop\Apps\BookManager\DB\db.db";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">DBファイルのパス(省略で本番ファイル)</param>
        public SqlitePositionDataAccess(string? connectionString = null)
        {
            if (connectionString != null)
            {
                this.dbFilePath = connectionString;
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
                using (var command = new SqliteCommand(quely, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var position = new PositionData();
                            position.Position = (string)reader["position"];
                            result.Add(position);
                        }
                    }
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
            var quely = $"INSERT INTO position (position) VALUES ('{position.Position}')";
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
        /// 配置を削除
        /// </summary>
        /// <param name="position">配置</param>
        public void DeletePosition(PositionData position)
        {
            var quely = $"DELETE FROM position WHERE position = '{position.Position}'";
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
