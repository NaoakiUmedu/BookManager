namespace BookManager.Position
{
    /// <summary>
    /// 配置に関するユースケースのインタフェース(現状主キーのみなのでUpdateはありえない)
    /// </summary>
    public interface IPositionUsecase
    {
        /// <summary>
        /// 配置を挿入
        /// </summary>
        /// <param name=""></param>
        public void Insert(List<PositionData> data);
        /// <summary>
        /// 全件読み込み
        /// </summary>
        /// <returns>データ</returns>
        public List<PositionData> Read();
        /// <summary>
        /// 指定した配置を削除
        /// </summary>
        /// <param name="data"></param>
        public void Delete(List<PositionData> data);
    }
}
