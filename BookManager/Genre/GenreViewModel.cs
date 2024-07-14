using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Genre
{
    internal class GenreViewModel
    {
        /// <summary>
        /// ジャンル
        /// </summary>
        internal class GenreViewData
        {
            public string GenreName { get; set; } = string.Empty;
        }

        /// <summary>
        /// ジャンル
        /// </summary>
        public ObservableCollection<GenreViewData> Genres { get; set; } = new ObservableCollection<GenreViewData>();
    }
}
