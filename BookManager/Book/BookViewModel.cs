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
using static System.Reflection.Metadata.BlobBuilder;

namespace BookManager.Book
{
    /// <summary>
    /// 蔵書一覧画面のViewModel
    /// </summary>
    internal class BookViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 蔵書一覧モデルクラス
        /// </summary>
        private readonly IBookUsecase bookUsecaseImpl = new BookUsecaseImpl();
        /// <summary>
        /// 段ボールモデルクラス
        /// </summary>
        private readonly Box.IBoxUsecase boxUsecaseImpl = new Box.BoxUsecaseImpl();
        /// <summary>
        /// ジャンルモデルクラス
        /// </summary>
        private readonly Genre.IGenreUsecase genreUsecaseImpl = new Genre.GenreUsecaseImpl();
        /// <summary>
        /// 配置モデルクラス
        /// </summary>
        private readonly Position.IPositionUsecase positionUsecaseImpl = new Position.PositionUsecaseImpl();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BookViewModel()
        {

        }

        /// <summary>
        /// コンストラクタ(依存性注入用)
        /// </summary>
        /// <param name="bookUsecaseImpl">蔵書一覧モデルクラス</param>
        public BookViewModel(
            IBookUsecase? bookUsecaseImpl = null,
            Box.IBoxUsecase? boxUsecaseImpl = null,
            Genre.IGenreUsecase? genreUsecaseImpl = null,
            Position.IPositionUsecase? positionUsecaseImpl = null)
        {
            this.bookUsecaseImpl = bookUsecaseImpl ?? this.bookUsecaseImpl;
            this.boxUsecaseImpl = boxUsecaseImpl ?? this.boxUsecaseImpl;
            this.genreUsecaseImpl = genreUsecaseImpl ?? this.genreUsecaseImpl;
            this.positionUsecaseImpl = positionUsecaseImpl ?? this.positionUsecaseImpl;
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
            public Guid Id = Guid.NewGuid();
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
            bookUsecaseImpl.Insert(willInsert.ToList());

            // 更新
            var willUpdate = from book in BookViewDatas
                             where book.Operation == OPERATION.UPDATE.ToString()
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box };
            bookUsecaseImpl.Update(willUpdate.ToList());

            // 削除
            var willDelete = from book in BookViewDatas
                             where book.Operation == OPERATION.DELETE.ToString()
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box };
            bookUsecaseImpl.Delete(willDelete.ToList());

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
            var nowBoxes = boxUsecaseImpl.Read();
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
                boxUsecaseImpl.Insert(notExistBoxes.Distinct().ToList());
            }
        }

        /// <summary>
        /// ジャンルを挿入
        /// </summary>
        private void InsertGenre()
        {
            var nowGenres = genreUsecaseImpl.Read();
            var notExistGenres = new List<GenreData>();
            foreach (var book in BookViewDatas)
            {
                if (!(nowGenres.Exists(x => x.GenreName == book.Genre)))
                {
                    notExistGenres.Add(new() { GenreName = book.Genre });
                }
            }
            
            if (notExistGenres.Count > 0)
            {
                genreUsecaseImpl.Insert(notExistGenres.Distinct().ToList());
            }
        }

        /// <summary>
        /// 配置を挿入
        /// </summary>
        private void InsertPosition()
        {
            var nowPositions = positionUsecaseImpl.Read();
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
                positionUsecaseImpl.Insert(notExistPositions.Distinct().ToList());
            }
        }

        /// <summary>
        /// 本の一覧を読み込み
        /// </summary>
        internal void ReadBook()
        {

            var books = bookUsecaseImpl.Read();

            // 注意!丸々置き換えるとBindingが解ける!
            // Concatでもだめらしい
            BookViewDatas.Clear();
            foreach(var book in books)
            {
                BookViewDatas.Add(new BookViewData() {
                    Id=book.Id,
                    Auther=book.Auther,
                    BookName=book.BookName,
                    Box=book.Box,
                    Genre=book.Genre,
                    Position=book.Position });
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
            var boxes = boxUsecaseImpl.Read();
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
            var genres = genreUsecaseImpl.Read();
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
            var positions = positionUsecaseImpl.Read();
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

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void Import(string filePath)
        {
            var imported = bookUsecaseImpl.Import(filePath);
            // 注意!丸々置き換えるとBindingが解ける!
            // Concatでもだめらしい
            BookViewDatas.Clear();
            foreach (var book in imported)
            {
                BookViewDatas.Add(new BookViewData() {
                    Operation = OPERATION.INSERT.ToString(),
                    Id = book.Id,
                    Auther = book.Auther,
                    BookName = book.BookName,
                    Box = book.Box,
                    Genre = book.Genre,
                    Position = book.Position });
            }
            // プルダウンに追加して画面に出せるようにする(まだDataAccessには突っ込まない)
            AddNotExistPulldown();
        }

        /// <summary>
        /// BookViewDatasに入っているがまだ選択肢にないプルダウンを追加する(インポート時に使う)
        /// </summary>
        private void AddNotExistPulldown()
        {
            AddNotExistBox();
            AddNotExistGenre();
            AddNotExistPosition();
        }

        /// <summary>
        /// 選択肢にない段ボールを選択肢に追加
        /// </summary>
        private void AddNotExistBox()
        {
            foreach (var book in BookViewDatas)
            {
                if (!(BoxChoces.ToList().Exists(x => x == book.Box)))
                {
                    BoxChoces.Add(book.Box);
                }
            }
        }

        /// <summary>
        /// 選択肢にないジャンルを選択肢に追加
        /// </summary>
        private void AddNotExistGenre()
        {
            foreach (var book in BookViewDatas)
            {
                if (!(GenreChoces.ToList().Exists(x => x == book.Genre)))
                {
                    GenreChoces.Add(book.Genre);
                }
            }
        }

        /// <summary>
        /// 選択肢にない配置を選択肢に追加
        /// </summary>
        private void AddNotExistPosition()
        {
            foreach (var book in BookViewDatas)
            {
                if (!(PositionChoces.ToList().Exists(x => x == book.Position)))
                {
                    PositionChoces.Add(book.Position);
                }
            }
        }

        /// <summary>
        /// エクスポート
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void Export(string filePath)
        {
            var willExport = from book in BookViewDatas
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box };
            bookUsecaseImpl.Export(filePath, willExport.ToList());
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="keyword">キーワード</param>
        public void Seach(string keyword)
        {
            var searched = (from book in BookViewDatas
                           where book.BookName.Contains(keyword)
                           select new BookViewData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.Box }).ToList();
            BookViewDatas.Clear();
            foreach (var book in searched)
            {
                BookViewDatas.Add(book);
            }
        }
    }
}
