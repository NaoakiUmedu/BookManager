namespace BookManager.Position
{
    /// <summary>
    /// 配置に関するユースケースの実装
    /// </summary>
    internal class PositionUsecaseImpl : IPositionUsecase
    {
        /// <summary>
        /// データアクセッサ(デフォルトでは本番環境)
        /// </summary>
        private IPositionDataAccess dataAccess = new SqlitePositionDataAccess();
        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="dataAccess">データアクセッサ</param>
        public PositionUsecaseImpl(IPositionDataAccess? dataAccess = null)
        {
            this.dataAccess = dataAccess ?? this.dataAccess;
        }

        /// <summary>
        /// 配置を挿入
        /// </summary>
        /// <param name=""></param>
        public void Insert(List<PositionData> data)
        {
            foreach (var datum in data)
            {
                dataAccess.InsertPosition(datum);
            }
        }
        /// <summary>
        /// 全件読み込み
        /// </summary>
        /// <returns>データ</returns>
        public List<PositionData> Read()
        {
            return dataAccess.SelectAllPosition();
        }
        /// <summary>
        /// 指定した配置を削除
        /// </summary>
        /// <param name="data"></param>
        public void Delete(List<PositionData> data)
        {
            foreach (var datum in data)
            {
                dataAccess.DeletePosition(datum);
            }
        }
    }
}
