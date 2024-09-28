namespace BookManager.Genre
{
    /// <summary>
    /// ジャンルデータアクセッサのインターフェース
    /// </summary>
    public interface IGenreDataAccess
    {
        /// <summary>
        /// ジャンル全件取得
        /// </summary>
        /// <returns>ジャンル一覧</returns>
        public List<GenreData> SelectAllGenre();

        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void InsertGenre(GenreData genre);

        /// <summary>
        /// ジャンルを削除
        /// </summary>
        /// <param name="genre">ジャンル</param>
        public void DeleteGenre(GenreData genre);
    }
}
