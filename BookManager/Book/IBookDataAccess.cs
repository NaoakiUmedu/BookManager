namespace BookManager.Book
{
    /// <summary>
    /// 蔵書データアクセッサのインターフェース
    /// </summary>
    internal interface IBookDataAccess
    {
        /// <summary>
        /// 蔵書全件取得
        /// </summary>
        /// <returns>蔵書一覧</returns>
        public List<BookData> SelectAllBook();

        /// <summary>
        /// 蔵書1冊を挿入
        /// </summary>
        /// <param name="book">蔵書</param>
        public void InsertBook(BookData book);
        /// <summary>
        /// 蔵書1冊を削除
        /// </summary>
        /// <param name="book">蔵書</param>
        public void DeleteBook(BookData book);
        /// <summary>
        /// 蔵書1冊を更新
        /// </summary>
        /// <param name="book">蔵書</param>
        public void UpdateBook(BookData book);
    }
}
