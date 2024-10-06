namespace BookManager.Genre
{
    /// <summary>
    /// ジャンル一覧に関するユースケースの実装
    /// </summary>
    internal class GenreUsecaseImpl : IGenreUsecase
    {
        /// <summary>
        /// データアクセッサ(デフォルトでは本番環境)
        /// </summary>
        private readonly IGenreDataAccess dataAccess = new SqliteGenreDataAccess(isolationLevel:System.Data.IsolationLevel.Serializable);
        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="dataAccess">データアクセッサ</param>
        public GenreUsecaseImpl(IGenreDataAccess? dataAccess = null)
        {
            this.dataAccess = dataAccess ?? this.dataAccess;
        }

        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        /// <param name=""></param>
        public void Insert(List<GenreData> data)
        {
            foreach(var datum in data)
            {
                dataAccess.InsertGenre(datum);
            }
        }
        /// <summary>
        /// 全件読み込み
        /// </summary>
        /// <returns>データ</returns>
        public List<GenreData> Read()
        {
            return dataAccess.SelectAllGenre();
        }
        /// <summary>
        /// 指定したジャンルを削除
        /// </summary>
        /// <param name="data"></param>
        public void Delete(List<GenreData> data)
        {
            foreach (var datum in data)
            {
                dataAccess.DeleteGenre(datum);
            }
        }
    }
}
