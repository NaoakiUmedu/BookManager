namespace BookManager.Genre
{
    /// <summary>
    /// ジャンルを表すデータ
    /// </summary>
    public class GenreData : IEquatable<GenreData>
    {
        public string GenreName = string.Empty;

        public bool Equals(GenreData? other)
        {
            if (other is null)
                return false;

            return this.GenreName == other.GenreName;
        }

        public override bool Equals(object? obj) => Equals(obj as GenreData);
        public override int GetHashCode() => (GenreName).GetHashCode();
    }
}
