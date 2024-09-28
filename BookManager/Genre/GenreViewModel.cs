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
        /// <summary>
        /// モデル
        /// </summary>
        private readonly IGenreUsecase model = new GenreUsecaseImpl();
        /// <summary>
        /// コンストラクタ(本番用)
        /// </summary>
        public GenreViewModel()
        {

        }
        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="model">モデル</param>
        public GenreViewModel(IGenreUsecase model)
        {
            this.model = model;
        }


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
        public ObservableCollection<GenreViewData> GenreViewDatas { get; set; } = new ObservableCollection<GenreViewData>();
        /// <summary>
        /// プルダウンで追加だのなんだのやるのは面倒なので、差分を取っておく(どうせ追加と削除しかない)
        /// </summary>
        private List<GenreData> preData = [];

        /// <summary>
        /// 一行追加
        /// </summary>
        public void AddGenre()
        {
            GenreViewDatas.Add(new GenreViewData());
        }

        /// <summary>
        /// 指定したジャンル名の行を削除
        /// </summary>
        /// <param name="name">ジャンル名</param>
        public void DeleteGenre(string name)
        {
            for (int i = 0; i < GenreViewDatas.Count; i++)
            {
                if (name == GenreViewDatas[i].GenreName)
                {
                    GenreViewDatas.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        internal void Save()
        {
            var updateData = from genre in GenreViewDatas
                             select new GenreData() { GenreName = genre.GenreName };

            var insertedData = updateData.Except(preData);
            var deletedData = preData.Except(updateData);

            model.Insert(insertedData.ToList());
            model.Delete(deletedData.ToList());

            // preDataを更新
            preData = updateData.ToList();
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        internal void Read()
        {
            var readGenre = model.Read();
            var readGenreViewData = from genre in readGenre
                                    select new GenreViewData() { GenreName = genre.GenreName };
            
            // べつのものを代入するとBindingが解ける
            // Concatでもだめらしい
            GenreViewDatas.Clear();
            foreach(var datum in readGenreViewData)
            {
                GenreViewDatas.Add(datum);
            }

            // Saveで差分を取るため、前回値を保存
            preData = readGenre;
        }
    }
}
