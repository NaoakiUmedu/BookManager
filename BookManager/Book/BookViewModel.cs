using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookManager.Book
{
    internal class BookViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 蔵書一覧モデルクラス
        /// </summary>
        private readonly IBookModel model = new BookModel();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BookViewModel()
        {

        }

        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="model">蔵書一覧モデルクラス</param>
        public BookViewModel(IBookModel model)
        {
            this.model = model;
        }

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
            public string Box { get; set; } = string.Empty;
        }
        /// <summary>
        /// 画面表示データ
        /// </summary>
        public ObservableCollection<BookViewData> BookViewDatas { get; set; } = new ();

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
            BookViewDatas.Add(new BookViewData() { Operation = OPERATION.INSERT.ToString() });
        }

        /// <summary>
        /// 当該IDを削除
        /// </summary>
        /// <param name="id">削除する本のID</param>
        internal void DeleteBook(Guid id)
        {
            for (int i = 0; i < BookViewDatas.Count; i++)
            {
                if (id == BookViewDatas[i].Id)
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
            var books = from book in BookViewDatas
                        select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.BookName };

            model.Save(books.ToList());
        }

        /// <summary>
        /// 本の一覧を読み込み
        /// </summary>
        internal void ReadBook()
        {

            var books = model.Read();
            var bookList = from book in books
                            select new BookViewData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Box = book.BookName, Position = book.Position };

            // 注意!丸々置き換えるとBindingが解ける!
            BookViewDatas.Clear();
            BookViewDatas.Concat(bookList);
        }

        /// <summary>
        /// 操作プルダウンの選択肢
        /// </summary>
        public ObservableCollection<String> OperationChoces { get; set; } = new ObservableCollection<string>()
        {
            OPERATION.NONE.ToString(),
            OPERATION.INSERT.ToString(),
            OPERATION.UPDATE.ToString(),
            OPERATION.DELETE.ToString(),
        };


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
