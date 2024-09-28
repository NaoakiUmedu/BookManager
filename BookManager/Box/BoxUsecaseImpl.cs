namespace BookManager.Box
{
    /// <summary>
    /// 段ボールに関するユースケースの実装
    /// </summary>
    internal class BoxUsecaseImpl : IBoxUsecase
    {
        /// <summary>
        /// データアクセッサ(デフォルトでは本番環境)
        /// </summary>
        private IBoxDataAccess dataAccess = new SqliteBoxDataAccess();
        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="dataAccess">データアクセッサ</param>
        public BoxUsecaseImpl(IBoxDataAccess? dataAccess = null)
        {
            this.dataAccess = dataAccess ?? this.dataAccess;
        }

        /// <summary>
        /// 段ボールを挿入
        /// </summary>
        /// <param name=""></param>
        public void Insert(List<BoxData> data)
        {
            foreach(var datum in data)
            {
                dataAccess.InsertBox(datum);
            }
        }
        /// <summary>
        /// 全件読み込み
        /// </summary>
        /// <returns>データ</returns>
        public List<BoxData> Read()
        {
            return dataAccess.SelectAllBox();
        }
        /// <summary>
        /// 指定した段ボールを削除
        /// </summary>
        /// <param name="data"></param>
        public void Delete(List<BoxData> data)
        {
            foreach (var datum in data)
            {
                dataAccess.DeleteBox(datum);
            }
        }
    }
}
