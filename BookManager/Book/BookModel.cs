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
        /// DB用データアクセッサ
        /// </summary>
        private IBookDataAccess dbDataAccesser = new SqliteBookDataAccess();
        /// <summary>
        /// インポート・エクスポート用データアクセッサ
        /// </summary>
        private IImportExportBookDataAccess importExportDataAccesser = new TsvBookDataAccess();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbDa">データアクセッサ</param>
        /// <param name="importExportDataAccesser">インポート・エクスポート用データアクセッサ</par
        public BookModel(
            IBookDataAccess? dbDa = null,
            IImportExportBookDataAccess? importExportDataAccesser = null)
        {
            this.dbDataAccesser = dbDa ?? this.dbDataAccesser;
            this.importExportDataAccesser = importExportDataAccesser ?? this.importExportDataAccesser;
        }

        /// <summary>
        /// 本の一覧を読みこむ
        /// </summary>
        /// <returns></returns>
        public List<BookData> Read()
        {
            return dbDataAccesser.SelectAllBook();
        }

        /// <summary>
        /// 本を追加する
        /// </summary>
        /// <param name="books">本</param>
        public void Insert(List<BookData> books)
        {
            foreach(var book in books)
            {
                dbDataAccesser.InsertBook(book);
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
                dbDataAccesser.UpdateBook(book);
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
                dbDataAccesser.DeleteBook(book);
            }
        }

        /// <summary>
        /// ファイルエクスポート
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="books">本</param>
        public void Export(string filePath, List<BookData> books)
        {
            importExportDataAccesser.ExportBooks(filePath, books);
        }

        /// <summary>
        /// ファイルインポート
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>本</returns>
        public List<BookData> Import(string filePath)
        {
            return importExportDataAccesser.ImportBooks(filePath);
        }
    }
}
