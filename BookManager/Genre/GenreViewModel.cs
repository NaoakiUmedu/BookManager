using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static BookManager.Book.BookViewModel;

namespace BookManager.Genre
{
    internal class GenreViewModel : INotifyPropertyChanged
    {
        // INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        /// <summary>
        /// 一行追加
        /// </summary>
        public void AddGenre()
        {
            Genres.Add(new GenreViewData());
        }

        /// <summary>
        /// 指定したジャンル名の行を削除
        /// </summary>
        /// <param name="name">ジャンル名</param>
        public void DeleteGenre(string name)
        {
            for (int i = 0; i < Genres.Count; i++)
            {
                if (name == Genres[i].GenreName)
                {
                    Genres.RemoveAt(i);
                }
            }
        }
    }
}
