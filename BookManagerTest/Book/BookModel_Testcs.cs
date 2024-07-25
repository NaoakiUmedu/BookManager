using BookManager.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerTest.Book
{
    [TestClass]
    public class BookModel_Testcs
    {
        /// <summary>
        /// BookDataAccessのスタブ
        /// </summary>
        private class StubBookDataAccess : IBookDataAccess
        {
            private List<BookData>  books = new List<BookData>();
            /// <summary>
            /// 蔵書全件取得
            /// </summary>
            /// <returns>蔵書一覧</returns>
            public List<BookData> SelectAllBooks()
            {
                return books;
            }

            /// <summary>
            /// 蔵書1冊を挿入
            /// </summary>
            /// <param name="book">蔵書</param>
            public void InsertBook(BookData book) { books.Add(book); }
            /// <summary>
            /// 蔵書1冊を削除
            /// </summary>
            /// <param name="book">蔵書<param>
            public void DeleteBook(BookData book)
            {
                for(int i = 0; i < books.Count; i++)
                {
                    if(book.Id == books[i].Id)
                    {
                        books.RemoveAt(i);
                    }
                }
            }
            /// <summary>
            /// 蔵書1冊を更新
            /// </summary>
            /// <param name="book">蔵書<param>
            public void UpdateBook(BookData book)
            {
                for (int i = 0; i < books.Count; i++)
                {
                    if (book.Id == books[i].Id)
                    {
                        books[i] = book;
                    }
                }
            }
        }

        /// <summary>
        /// 蔵書を保存・検索するテスト
        /// </summary>
        [TestMethod]
        public void InsertBook_Test()
        {
            // Arrange
            var bookmodel = new BookModel(dataAccess:new StubBookDataAccess());
            var books = new List<BookData>()
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

            // Act
            bookmodel.Insert(books);

            // Assert
            var readed = bookmodel.Read();
            for(var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(books[i].Id, readed[i].Id);
                Assert.AreEqual(books[i].BookName, readed[i].BookName);
                Assert.AreEqual(books[i].Auther, readed[i].Auther);
                Assert.AreEqual(books[i].Genre, readed[i].Genre);
                Assert.AreEqual(books[i].Position, readed[i].Position);
                Assert.AreEqual(books[i].Box, readed[i].Box);
            }
        }

        /// <summary>
        /// 本を更新するテスト
        /// </summary>
        public void UpdateBook_Test()
        {
            var id = Guid.NewGuid();
            var books = new List<BookData>()
            {
                new BookData()
                {
                    Id = id,
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                }
            };

            var updatedBooks = new List<BookData>(){new BookData()
            {
                Id = id,
                BookName = "ある明治人の記録",
                Auther = "柴五郎",
                Genre = "歴史",
                Position = "本棚(小)",
                Box = "所属段ボール"
            } };

            // Arrange
            var bookmodel = new BookModel(dataAccess: new StubBookDataAccess());
            bookmodel.Insert(books);
            bookmodel.Update(updatedBooks);

            // Assert
            var readed = bookmodel.Read();
            Assert.AreEqual(updatedBooks.Count, readed.Count);
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(updatedBooks[i].Id, readed[i].Id);
                Assert.AreEqual(updatedBooks[i].BookName, readed[i].BookName);
                Assert.AreEqual(updatedBooks[i].Auther, readed[i].Auther);
                Assert.AreEqual(updatedBooks[i].Genre, readed[i].Genre);
                Assert.AreEqual(updatedBooks[i].Position, readed[i].Position);
                Assert.AreEqual(updatedBooks[i].Box, readed[i].Box);
            }
        }

        /// <summary>
        /// 蔵書を削除するテスト
        /// </summary>
        [TestMethod]
        public void DeleteBook_Test()
        {
            // Arrange
            var deleteId = Guid.NewGuid();
            var bookmodel = new BookModel(dataAccess: new StubBookDataAccess());
            var books = new List<BookData>()
            {
                new BookData()
                {
                    Id = deleteId,
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

            var deleteBooks = new List<BookData>()
            { 
                new BookData(){Id = deleteId}
            };

            // Act
            bookmodel.Insert(books);
            bookmodel.Delete(deleteBooks);

            // Assert
            var readed = bookmodel.Read();
            Assert.AreEqual(1, readed.Count);
            for (var i = 0; i < readed.Count; i++)
            {
                Assert.AreEqual(books[1].Id, readed[i].Id);
                Assert.AreEqual(books[1].BookName, readed[i].BookName);
                Assert.AreEqual(books[1].Auther, readed[i].Auther);
                Assert.AreEqual(books[1].Genre, readed[i].Genre);
                Assert.AreEqual(books[1].Position, readed[i].Position);
                Assert.AreEqual(books[1].Box, readed[i].Box);
            }
        }
    }
}
