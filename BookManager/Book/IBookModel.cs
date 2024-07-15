using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Book
{
    public interface IBookModel
    {

        /// <summary>
        /// 本の一覧を保存する
        /// </summary>
        public void Save(List<BookData> books);

        /// <summary>
        /// 本の一覧を読みこむ
        /// </summary>
        /// <returns></returns>
        public List<BookData> Read();
    }
}
