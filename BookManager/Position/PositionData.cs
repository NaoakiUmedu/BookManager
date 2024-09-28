namespace BookManager.Position
{
    /// <summary>
    /// 配置を表すデータ
    /// </summary>
    public class PositionData : IEquatable<PositionData>
    {
        public string Position = string.Empty;

        public bool Equals(PositionData? other)
        {
            if (other is null)
                return false;

            return this.Position == other.Position;
        }

        public override bool Equals(object? obj) => Equals(obj as PositionData);
        public override int GetHashCode() => (Position).GetHashCode();
    }
}
