using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace BookManager.Book
{
    internal class BookViewModel : INotifyPropertyChanged
    {
        private readonly BookModel model = new BookModel();

        /// <summary>
        /// 蔵書一覧画面 画面表示データクラス
        /// </summary>
        public class BookViewData
        {
            public string Operation { get; set; } = OPERATION.NONE.ToString();
            /// <summary>
            /// 書籍ID
            /// </summary>
            public Guid Id;
            /// <summary>
            /// 書名
            /// </summary>
            public string BookName { get; set; } = string.Empty;
            /// <summary>
            /// 著者名
            /// </summary>
            public string Auther { get; set; } = string.Empty;
            /// <summary>
            /// ジャンル
            /// </summary>
            public string Genre { get; set; } = string.Empty;
            /// <summary>
            /// 配置
            /// </summary>
            public string Position { get; set; } = string.Empty;
            /// <summary>
            /// 所属段ボール
            /// </summary>
            public string Carbonboard { get; set; } = string.Empty;
        }
        /// <summary>
        /// 画面表示データ
        /// </summary>
        public ObservableCollection<BookViewData> BookViewDatas { get; set; } = new ObservableCollection<BookViewData>()
        {
            // 以下はレイアウト確認用のダミーデータ
            new BookViewData(){Operation = OPERATION.NONE.ToString(), Id = Guid.NewGuid(), BookName="新世界より1", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Carbonboard="文庫1(エンタメ)"},
            new BookViewData(){Operation = OPERATION.INSERT.ToString(), Id = Guid.NewGuid(), BookName="新世界より2", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Carbonboard="文庫1(エンタメ)"},
            new BookViewData(){Operation = OPERATION.UPDATE.ToString(), Id = Guid.NewGuid(), BookName="新世界より3", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Carbonboard="文庫1(エンタメ)"}
        };

        // INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 1行追加
        /// </summary>
        internal void AddBook()
        {
            BookViewDatas.Add(new BookViewData() { Operation = OPERATION.INSERT.ToString()});
        }

        /// <summary>
        /// 当該IDを削除
        /// </summary>
        /// <param name="id">削除する本のID</param>
        internal void DeleteBook(Guid id)
        {
            for(int i = 0; i < BookViewDatas.Count; i++)
            {
                if(id == BookViewDatas[i].Id)
                {
                    BookViewDatas.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 本を保存する
        /// </summary>
        internal void SaveBook()
        {
            // TODO 保存する
        }

        /// <summary>
        /// 操作プルダウンの選択肢
        /// </summary>
        public static ObservableCollection<String> OperationChoces { get; set; } =
        [
            OPERATION.NONE.ToString(),
            OPERATION.INSERT.ToString(),
            OPERATION.UPDATE.ToString(),
            OPERATION.DELETE.ToString(),
        ];


        /// <summary>
        /// 操作を表すenum
        /// </summary>
        public enum OPERATION
        {
            /// <summary>
            /// なにもしない
            /// </summary>
            NONE,
            /// <summary>
            /// 追加
            /// </summary>
            INSERT,
            /// <summary>
            /// 更新
            /// </summary>
            UPDATE,
            /// <summary>
            /// 削除
            /// </summary>
            DELETE
        };
    }
}
