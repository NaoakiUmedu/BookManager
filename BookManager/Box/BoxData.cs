using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookManager.Box
{
    /// <summary>
    /// 段ボールを表すデータ
    /// </summary>
    public class BoxData : IEquatable<BoxData>
    {
        public string BoxName = string.Empty;

        public bool Equals(BoxData? other)
        {
            if (other is null)
                return false;

            return this.BoxName == other.BoxName;
        }

        public override bool Equals(object? obj) => Equals(obj as BoxData);
        public override int GetHashCode() => (BoxName).GetHashCode();
    }
}
