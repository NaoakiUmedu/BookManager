using BookManager.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Genre
{
    internal interface IGenreDataAccess
    {
        /// <summary>
        /// ジャンル全件取得
        /// </summary>
        /// <returns>ジャンル一覧</returns>
        public List<GenreData> SelectAllGenre();

        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void InsertGenre(GenreData genre);

        /// <summary>
        /// ジャンルを削除
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void DeleteGenre(GenreData genre);
    }
}
