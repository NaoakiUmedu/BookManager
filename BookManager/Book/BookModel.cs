using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    internal class BookModel : IBookModelInterface
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
        /// 本の一覧を保存する
        /// </summary>
        public void Save(List<BookData> books)
        {
            foreach(var book in books)
            {
                dataAccess.InsertBook(book);
            }
        }

        /// <summary>
        /// 本の一覧を読みこむ
        /// </summary>
        /// <returns></returns>
        public List<BookData> Read()
        {
            return dataAccess.SelectAllBooks();
        }
    }
}
