using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Genre
{
    internal class SqliteGenreDataAccess : IGenreDataAccess
    {
        /// <summary>
        /// DBファイルのパス(デフォルトは本番DB)
        /// </summary>
        private string dbFilePath = @"Data Source=C:\Users\anija\Desktop\Apps\BookManager\DB\db.db";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">DBファイルのパス(省略で本番ファイル)</param>
        public SqliteGenreDataAccess(string? connectionString = null)
        {
            if (connectionString != null)
            {
                this.dbFilePath = connectionString;
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
                using (var command = new SqliteCommand(quely, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var genre = new GenreData();
                            genre.GenreName = (string)reader["genrename"];
                            result.Add(genre);
                        }
                    }
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
            var quely = $"INSERT INTO genre (genrename) VALUES ('{genre.GenreName}')";
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
        /// ジャンルを削除
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void DeleteGenre(GenreData genre)
        {
            var quely = $"DELETE FROM genre WHERE genrename = '{genre.GenreName}'";
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
