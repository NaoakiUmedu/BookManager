using BookManager.Book;
using System.IO;
using System.Windows.Shapes;

namespace BookManagerTest.Book
{
    [TestClass]
    public class TsvBookDataAccess_Test
    {
        [TestMethod]
        public void Import_Test()
        {
            WriteTextFile(testFilePath, tsvTexts);

            var da = new TsvBookDataAccess();
            var imported = da.ImportBooks(testFilePath);

            Assert.AreEqual(books.Count, imported.Count);
            for(int i = 0; i < books.Count; i++)
            {
                Assert.AreEqual(books[i].BookName, imported[i].BookName);
                Assert.AreEqual(books[i].Auther, imported[i].Auther);
                Assert.AreEqual(books[i].Genre, imported[i].Genre);
                Assert.AreEqual(books[i].Position, imported[i].Position);
                Assert.AreEqual(books[i].Box, imported[i].Box);
            }

            DeleteFile(testFilePath);
        }

        [TestMethod]
        public void Export_Test()
        {
            var da = new TsvBookDataAccess();
            da.ExportBooks(testFilePath, books);

            var readed = TextFileRead(testFilePath);

            Assert.AreEqual(tsvTexts.Count, readed.Count);
            for (int i = 0; i < books.Count; i++)
            {
                Assert.AreEqual(tsvTexts[i], readed[i]);
            }

            DeleteFile(testFilePath);
        }

        private List<BookData> books = new ()
        {
            new BookData()
            {
                BookName = "ある明治人の記録",
                Auther = "柴五郎",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "新書1"
            },
            new BookData()
            {
                BookName = "中世への旅　農民戦争と傭兵",
                Auther = "ハインリヒ ブレティヒャ",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "新書1"
            }
        };

        private List<String> tsvTexts = new()
        {
            "ある明治人の記録	柴五郎	歴史	本棚(小)	新書1",
            "中世への旅　農民戦争と傭兵	ハインリヒ ブレティヒャ	歴史	本棚(小)	新書1"
        };

        private string testFilePath = @"C:\Users\anija\Desktop\codes\BookManager\BookManagerTest\TestDb\Test.tsv";

        private List<String> TextFileRead(string filePath)
        {
            List<String> contents = new();

            StreamReader sr = new StreamReader(filePath);
            //Read the first line of text
            var line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                contents.Add(line);
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();

            return contents;
        }

        private void WriteTextFile(string filePath, List<String> contents)
        {
            var sw = new StreamWriter(filePath);
            foreach (var line in contents)
            {
                sw.WriteLine(line);
            };
            sw.Close();
        }

        private void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
