using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BookManager.Book
{
    internal class TsvBookDataAccess : IImportExportBookDataAccess
    {
        /// <summary>
        /// 蔵書全件取得
        /// </summary>
        /// <returns>蔵書一覧</returns>
        public List<BookData> ImportBooks(string filePath)
        {
            var result = new List<BookData>();
            var readed = TextFileRead(filePath);
            foreach(var line in readed)
            {
                List<string> splitted = line.Split(separator).ToList();
                if(splitted.Count != parsedSize)
                {
                    continue;
                }

                result.Add(ParseLine(splitted));
            }
            return result;
        }

        /// <summary>
        /// 蔵書1冊を更新
        /// </summary>
        /// <param name="book">蔵書</param>
        public void ExportBooks(string filePath, List<BookData> books)
        {
            var contents = books.Select(x => SynthesizeLine(x)).ToList();
            WriteTextFile(filePath, contents);
        }

        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <param name="filePath">パス</param>
        /// <returns>ファイルの中身</returns>
        private List<String> TextFileRead(string filePath)
        {
            List<String> contents = new();

            StreamReader sr = new StreamReader(filePath);
            var line = sr.ReadLine();
            while (line != null)
            {
                contents.Add(line);
                //Read the next columns
                line = sr.ReadLine();
            }
            sr.Close();

            return contents;
        }

        /// <summary>
        /// ファイル書き込み
        /// </summary>
        /// <param name="filePath">パス</param>
        /// <param name="contents">書き込む中身</param>
        private void WriteTextFile(string filePath, List<String> contents)
        {
            var sw = new StreamWriter(filePath);
            foreach (var line in contents)
            {
                sw.WriteLine(line);
            };
            sw.Close();
        }

        /// <summary>
        /// 文字列のリストをBookDataにする
        /// </summary>
        /// <param name="columns">文字列のリスト</param>
        /// <returns>BookData</returns>
        private BookData ParseLine(List<string> columns)
        {
            return new BookData()
            {
                BookName = columns[0],
                Auther = columns[1],
                Genre = columns[2],
                Position = columns[3],
                Box = columns[4]
            };
        }

        /// <summary>
        /// BookDataを文字列にする
        /// </summary>
        /// <param name="book">本</param>
        /// <returns>文字列</returns>
        private string SynthesizeLine(BookData book)
        {
            var line = string.Empty;
            line += book.BookName;
            line += separator;
            line += book.Auther;
            line += separator;
            line += book.Genre;
            line += separator;
            line += book.Position;
            line += separator;
            line += book.Box;
            return line;
        }

        /// <summary>
        /// 区切り文字
        /// </summary>
        private char separator = '\t';
        /// <summary>
        /// 5列あるはず
        /// </summary>
        private int parsedSize = 5;
    }
}
