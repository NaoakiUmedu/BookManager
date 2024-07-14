using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    internal class BookModel
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
        /// 本を表すデータ
        /// </summary>
        public class BookData
        {
            /// <summary>
            /// 書籍ID
            /// </summary>
            public Guid Id = Guid.NewGuid();
            /// <summary>
            /// 書名
            /// </summary>
            public string BookName = string.Empty;
            /// <summary>
            /// 著者名
            /// </summary>
            public string Auther = string.Empty;
            /// <summary>
            /// ジャンル
            /// </summary>
            public string Genre = string.Empty;
            /// <summary>
            /// 配置
            /// </summary>
            public string Position = string.Empty;
            /// <summary>
            /// 所属段ボール
            /// </summary>
            public string Box = string.Empty;
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
