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
using BookManager.Box;
using BookManager.Genre;
using BookManager.Position;

namespace BookManager.Book
{
    internal class BookViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 蔵書一覧モデルクラス
        /// </summary>
        private readonly IBookModel bookModel = new BookModel();
        /// <summary>
        /// 段ボールモデルクラス
        /// </summary>
        private readonly Box.IBoxModel boxModel = new Box.BoxModel();
        /// <summary>
        /// ジャンルモデルクラス
        /// </summary>
        private readonly Genre.IGenreModel genreModel = new Genre.GenreModel();
        /// <summary>
        /// 配置モデルクラス
        /// </summary>
        private readonly Position.IPositionModel positionModel = new Position.PositionModel();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BookViewModel()
        {

        }

        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="bookModel">蔵書一覧モデルクラス</param>
        public BookViewModel(
            IBookModel? bookModel = null,
            Box.IBoxModel? boxModel = null,
            Genre.IGenreModel? genreModel = null,
            Position.IPositionModel? positionModel = null)
        {
            this.bookModel = bookModel ?? this.bookModel;
            this.boxModel = boxModel ?? this.boxModel;
            this.genreModel = genreModel ?? this.genreModel;
            this.positionModel = positionModel ?? this.positionModel;
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
        /// 本を保存する
        /// </summary>
        internal void SaveBook()
        {
            // 外部キーがなかったらInsertする(手入力ではありえないがインポート後に保存するとありうる)
            InsertReferenceKey();

            // 追加
            var willInsert = from book in BookViewDatas
                                        where book.Operation == OPERATION.INSERT.ToString()
                                        select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box };
            bookModel.Insert(willInsert.ToList());

            // 更新
            var willUpdate = from book in BookViewDatas
                             where book.Operation == OPERATION.UPDATE.ToString()
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box };
            bookModel.Update(willUpdate.ToList());

            // 削除
            var willDelete = from book in BookViewDatas
                             where book.Operation == OPERATION.DELETE.ToString()
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box };
            bookModel.Delete(willDelete.ToList());

            // 読み直す
            ReadBook();
        }

        /// <summary>
        /// 外部キーを挿入
        /// </summary>
        private void InsertReferenceKey()
        {
            InsertBox();
            InsertGenre();
            InsertPosition();
        }

        /// <summary>
        /// 段ボールを挿入
        /// </summary>
        private void InsertBox()
        {
            var nowBoxes = boxModel.Read();
            var notExistBoxes = new List<BoxData>();
            foreach(var book in BookViewDatas)
            {
                if (!(nowBoxes.Exists(x => x.BoxName== book.Box)))
                {
                    notExistBoxes.Add(new() { BoxName = book.Box });
                }
            }
            if(notExistBoxes.Count > 0)
            {
                boxModel.Insert(notExistBoxes);
            }
        }

        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        private void InsertGenre()
        {
            var nowGenres = genreModel.Read();
            var notExistGenres = new List<GenreData>();
            foreach (var book in BookViewDatas)
            {
                if (!(nowGenres.Exists(x => x.GenreName == book.Genre)))
                {
                    notExistGenres.Add(new() { GenreName = book.Genre });
                }
            }
            if(notExistGenres.Count > 0)
            {
                genreModel.Insert(notExistGenres);
            }
        }

        /// <summary>
        /// 配置を挿入
        /// </summary>
        private void InsertPosition()
        {
            var nowPositions = positionModel.Read();
            var notExistPositions = new List<PositionData>();
            foreach (var book in BookViewDatas)
            {
                if (!(nowPositions.Exists(x => x.Position == book.Position)))
                {
                    notExistPositions.Add(new() { Position = book.Position });
                }
            }
            if (notExistPositions.Count > 0)
            { 
                positionModel.Insert(notExistPositions);
            }
        }

        /// <summary>
        /// 本の一覧を読み込み
        /// </summary>
        internal void ReadBook()
        {

            var books = bookModel.Read();

            // 注意!丸々置き換えるとBindingが解ける!
            // Concatでもだめらしい
            BookViewDatas.Clear();
            foreach(var book in books)
            {
                BookViewDatas.Add(new BookViewData() {Id=book.Id, Auther=book.Auther, BookName=book.BookName, Box=book.Box, Genre=book.Genre, Position=book.Position });
            }
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
        /// 段ボールプルダウンの選択肢
        /// </summary>
        public ObservableCollection<String> BoxChoces { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// ジャンルプルダウンの選択肢
        /// </summary>
        public ObservableCollection<String> GenreChoces { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// 配置プルダウンの選択肢
        /// </summary>
        public ObservableCollection<String> PositionChoces { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// プルダウン更新
        /// </summary>
        public void UpdatePulldown()
        {
            UpdateBoxPulldown();
            UpdateGenrePulldown();
            UpdatePositionPulldown();
        }
        /// <summary>
        /// 段ボールプルダウン更新
        /// </summary>
        private void UpdateBoxPulldown()
        {
            var boxes = boxModel.Read();
            BoxChoces.Clear();
            foreach(var box in boxes)
            {
                BoxChoces.Add(box.BoxName);
            }
        }
        /// <summary>
        /// ジャンルプルダウン更新
        /// </summary>
        private void UpdateGenrePulldown()
        {
            var genres = genreModel.Read();
            GenreChoces.Clear();
            foreach(var genre in genres)
            {
                GenreChoces.Add(genre.GenreName);
            }
        }
        /// <summary>
        /// 配置プルダウン更新
        /// </summary>
        private void UpdatePositionPulldown()
        {
            var positions = positionModel.Read();
            PositionChoces.Clear();
            foreach(var position in positions)
            {
                PositionChoces.Add(position.Position);
            }
        }

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
