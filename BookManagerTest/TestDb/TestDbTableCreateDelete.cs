using Microsoft.Data.Sqlite;

namespace BookManagerTest.TestDb
{
    internal class TestDbTableCreateDelete
    {
        /// <summary>
        /// 単体テストはそれ用のDBでやる(DataAccessのDB指定方法をData Sourceの文字列にしているため、インメモリにできなかった！！！失敗！！！
        /// </summary>
        public static string TEST_DB_FILE_PATH = @"Data Source=..\..\..\..\BookManagerTest\TestDb\TestDb.db";

        /// <summary>
        /// テーブルの削除
        /// </summary>
        public static void DropTable()
        {
            var queries = new List<String>()
            {
                """DROP TABLE book;""",
                """DROP TABLE genre;""",
                """DROP TABLE position;""",
                """DROP TABLE box;"""
            };

            foreach (var query in queries)
            {
                try
                {
                    using (var connection = new SqliteConnection(TEST_DB_FILE_PATH))
                    {
                        connection.Open();

                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                }
                catch (Exception)
                {
                    // 前回テストが成功したときにCreateTableからよばれたら失敗する(意図通りの動作)
                }
            }
        }


        /// <summary>
        /// テーブルの作成
        /// </summary>
        public static void CreateTable()
        {
            // 前回テストが失敗しているとテーブルが残ったままになってる
            DropTable();

            try
            {

                using (var connection = new SqliteConnection(TEST_DB_FILE_PATH))
                {
                    connection.Open();

                    var query = """
                        -- ジャンル
                        create table genre (
                            -- ジャンル
                        	genrename   TEXT  primary key
                        );

                        -- 段ボール
                        create table box (
                            -- 段ボール
                        	boxname     TEXT    primary key
                        );

                        -- 配置
                        create table position (
                            -- 配置
                        	position    TEXT   primary key
                        );

                        -- 蔵書
                        create table book (
                            -- ID
                        	bookid      TEXT    primary  key,
                        	-- 書名
                        	bookname    TEXT,
                        	-- 著者名
                        	author      TEXT,
                        	-- ジャンル
                        	genre       TEXT    references genre(genrename) ON DELETE NO ACTION,
                        	-- 配置
                        	position    TEXT    references position(position) ON DELETE NO ACTION,
                        	-- 所属段ボール
                        	box         TEXT    references box(boxname)  ON DELETE NO ACTION
                        );
                        
                        """;
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {
                // 前回テストが失敗したときはテーブルが残っているので失敗する(意図通りの動作)
            }
        }
    }
}
