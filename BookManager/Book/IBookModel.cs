using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    public interface IBookModel
    {
        /// <summary>
        /// 本を追加する
        /// </summary>
        /// <param name="books">本</param>
        public void Insert(List<BookData> books);

        /// <summary>
        /// 本を更新する
        /// </summary>
        /// <param name="books">本</param>
        public void Update(List<BookData> books);

        /// <summary>
        /// 本を削除する
        /// </summary>
        /// <param name="books">本</param>
        public void Delete(List<BookData> books);

        /// <summary>
        /// 本の一覧を読みこむ
        /// </summary>
        /// <returns></returns>
        public List<BookData> Read();

        /// <summary>
        /// インポートする
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>本</returns>
        public List<BookData> Import(string filePath);

        /// <summary>
        /// エクスポートする
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="books">本</param>
        public void Export(string filePath, List<BookData> books);
    }
}
