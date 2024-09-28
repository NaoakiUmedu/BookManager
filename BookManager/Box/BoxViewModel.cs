using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static BookManager.Book.BookViewModel;

namespace BookManager.Box
{
    internal class BoxViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// モデル
        /// </summary>
        private readonly IBoxUsecase model = new BoxUsecaseImpl();
        /// <summary>
        /// コンストラクタ(本番用)
        /// </summary>
        public BoxViewModel()
        {

        }
        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="model">モデル</param>
        public BoxViewModel(IBoxUsecase model)
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
        /// 段ボール
        /// </summary>
        internal class BoxViewData
        {
            public string BoxName { get; set; } = string.Empty;
        }

        /// <summary>
        /// 段ボール
        /// </summary>
        public ObservableCollection<BoxViewData> BoxViewDatas { get; set; } = new ObservableCollection<BoxViewData>();
        /// <summary>
        /// プルダウンで追加だのなんだのやるのは面倒なので、差分を取っておく(どうせ追加と削除しかない)
        /// </summary>
        private List<BoxData> preData = [];

        /// <summary>
        /// 一行追加
        /// </summary>
        public void AddBox()
        {
            BoxViewDatas.Add(new BoxViewData());
        }

        /// <summary>
        /// 指定した段ボール名の行を削除
        /// </summary>
        /// <param name="name">段ボール名</param>
        public void DeleteBox(string name)
        {
            for (int i = 0; i < BoxViewDatas.Count; i++)
            {
                if (name == BoxViewDatas[i].BoxName)
                {
                    BoxViewDatas.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        internal void Save()
        {
            var updateData = from Box in BoxViewDatas
                             select new BoxData() { BoxName = Box.BoxName };

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
            var readBox = model.Read();
            var readBoxViewData = from Box in readBox
                                    select new BoxViewData() { BoxName = Box.BoxName };
            
            // べつのものを代入するとBindingが解ける
            // Concatでもだめらしい
            BoxViewDatas.Clear();
            foreach(var datum in readBoxViewData)
            {
                BoxViewDatas.Add(datum);
            }

            // Saveで差分を取るため、前回値を保存
            preData = readBox;
        }
    }
}
