namespace BookManager.Position
{
    /// <summary>
    /// 配置データアクセッサのインターフェース
    /// </summary>
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
