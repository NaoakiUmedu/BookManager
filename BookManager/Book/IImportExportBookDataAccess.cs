using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    public interface IImportExportBookDataAccess
    {
        /// <summary>
        /// 蔵書全件取得
        /// </summary>
        /// <returns>蔵書一覧</returns>
        public List<BookData> ImportBooks(string filePath);
        /// <summary>
        /// 蔵書1冊を更新
        /// </summary>
        /// <param name="book">蔵書</param>
        public void ExportBooks(string filePath, List<BookData> books);
    }
}
