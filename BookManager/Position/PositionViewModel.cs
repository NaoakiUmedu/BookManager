using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookManager.Position
{
    /// <summary>
    /// 配置一覧画面のViewModel
    /// </summary>
    internal class PositionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// モデル
        /// </summary>
        private readonly IPositionUsecase model = new PositionUsecaseImpl();
        /// <summary>
        /// コンストラクタ(本番用)
        /// </summary>
        public PositionViewModel()
        {

        }
        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="model">モデル</param>
        public PositionViewModel(IPositionUsecase model)
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
        /// 配置
        /// </summary>
        internal class PositionViewData
        {
            public string Position { get; set; } = string.Empty;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public ObservableCollection<PositionViewData> PositionViewDatas { get; set; } = new ObservableCollection<PositionViewData>();
        /// <summary>
        /// プルダウンで追加だのなんだのやるのは面倒なので、差分を取っておく(どうせ追加と削除しかない)
        /// </summary>
        private List<PositionData> preData = [];

        /// <summary>
        /// 一行追加
        /// </summary>
        public void AddPosition()
        {
            PositionViewDatas.Add(new PositionViewData());
        }

        /// <summary>
        /// 指定した配置名の行を削除
        /// </summary>
        /// <param name="name">配置名</param>
        public void DeletePosition(string name)
        {
            for (int i = 0; i < PositionViewDatas.Count; i++)
            {
                if (name == PositionViewDatas[i].Position)
                {
                    PositionViewDatas.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        internal void Save()
        {
            var updateData = from position in PositionViewDatas
                             select new PositionData() { Position = position.Position };

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
            var readPosition = model.Read();
            var readPositionViewData = from position in readPosition
                                    select new PositionViewData() { Position = position.Position };
            
            // べつのものを代入するとBindingが解ける
            // Concatでもだめらしい
            PositionViewDatas.Clear();
            foreach(var datum in readPositionViewData)
            {
                PositionViewDatas.Add(datum);
            }

            // Saveで差分を取るため、前回値を保存
            preData = readPosition;
        }
    }
}
