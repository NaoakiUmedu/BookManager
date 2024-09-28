using BookManager.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Box
{
    /// <summary>
    /// 段ボールのデータアクセッサのインターフェース
    /// </summary>
    public interface IBoxDataAccess
    {
        /// <summary>
        /// 段ボール全件取得
        /// </summary>
        /// <returns>段ボール一覧</returns>
        public List<BoxData> SelectAllBox();

        /// <summary>
        /// 段ボールを挿入
        /// </summary>
        /// <param name="Box">段ボール</param>
        public void InsertBox(BoxData Box);

        /// <summary>
        /// 段ボールを削除
        /// </summary>
        /// <param name="Box">段ボール</param>
        public void DeleteBox(BoxData Box);
    }
}
