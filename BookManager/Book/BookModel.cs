using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    internal class BookModel : IBookModel
    {
        /// <summary>
        /// データアクセッサ
        /// </summary>
        private IBookDataAccess dataAccess = new SqliteBookDataAccess();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataAccess">データアクセッサ</param>
        public BookModel(IBookDataAccess? dataAccess = null)
        {
            this.dataAccess = dataAccess ?? this.dataAccess;
        }

        /// <summary>
        /// 本の一覧を読みこむ
        /// </summary>
        /// <returns></returns>
        public List<BookData> Read()
        {
            return dataAccess.SelectAllBooks();
        }

        /// <summary>
        /// 本を追加する
        /// </summary>
        /// <param name="books">本</param>
        public void Insert(List<BookData> books)
        {
            foreach(var book in books)
            {
                dataAccess.InsertBook(book);
            }
        }

        /// <summary>
        /// 本を更新する
        /// </summary>
        /// <param name="books">本</param>
        public void Update(List<BookData> books)
        {
            foreach (var book in books)
            {
                dataAccess.UpdateBook(book);
            }
        }

        /// <summary>
        /// 本を削除する
        /// </summary>
        /// <param name="books">本</param>
        public void Delete(List<BookData> books)
        {
            foreach (var book in books)
            {
                dataAccess.DeleteBook(book);
            }
        }
    }
}
