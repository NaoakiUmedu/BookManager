using BookManager.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Position
{
    public interface IPositionDataAccess
    {
        /// <summary>
        /// 配置全件取得
        /// </summary>
        /// <returns>配置一覧</returns>
        public List<PositionData> SelectAllPosition();

        /// <summary>
        /// 配置を挿入
        /// </summary>
        /// <param name="position">配置</param>
        public void InsertPosition(PositionData position);

        /// <summary>
        /// 配置を削除
        /// </summary>
        /// <param name="position">配置</param>
        public void DeletePosition(PositionData position);
    }
}
