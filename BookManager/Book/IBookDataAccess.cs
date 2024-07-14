using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    internal interface IBookDataAccess
    {
        /// <summary>
        /// 蔵書全件取得
        /// </summary>
        /// <returns>蔵書一覧</returns>
        public List<BookModel.BookData> SelectAllBooks();

        /// <summary>
        /// 蔵書1冊を挿入
        /// </summary>
        /// <param name="book">蔵書</param>
        public void InsertBook(BookModel.BookData book);
    }
}
