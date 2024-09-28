using BookManager.Box;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookManager.Book
{

    /// <summary>
    /// 蔵書を表すデータ
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

        public bool Equals(BookData? other)
        {
            if (other is null)
                return false;

            return this.Id == other.Id;
        }

        public override bool Equals(object? obj) => Equals(obj as BookData);
        public override int GetHashCode() => (Id).GetHashCode();
    }

}
