﻿namespace BookManager.Genre
{
    /// <summary>
    /// ジャンルに関するユースケースのインタフェース(現状主キーのみなのでUpdateはありえない)
    /// </summary>
    public interface IGenreUsecase
    {
        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        /// <param name=""></param>
        public void Insert(List<GenreData> data);
        /// <summary>
        /// 全件読み込み
        /// </summary>
        /// <returns>データ</returns>
        public List<GenreData> Read();
        /// <summary>
        /// 指定したジャンルを削除
        /// </summary>
        /// <param name="data"></param>
        public void Delete(List<GenreData> data);
    }
}
