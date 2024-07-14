using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{

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

}
