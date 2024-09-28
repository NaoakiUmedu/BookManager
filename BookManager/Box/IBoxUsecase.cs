using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Box
{
    /// <summary>
    /// 段ボールに関するユースケースのインタフェース(現状主キーのみなのでUpdateはありえない)
    /// </summary>
    public interface IBoxUsecase
    {
        /// <summary>
        /// 段ボールを挿入
        /// </summary>
        /// <param name=""></param>
        public void Insert(List<BoxData> data);
        /// <summary>
        /// 全件読み込み
        /// </summary>
        /// <returns>データ</returns>
        public List<BoxData> Read();
        /// <summary>
        /// 指定した段ボールを削除
        /// </summary>
        /// <param name="data"></param>
        public void Delete(List<BoxData> data);
    }
}
