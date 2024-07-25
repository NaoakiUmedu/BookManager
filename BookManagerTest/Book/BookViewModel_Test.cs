using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BookManager.Book.BookViewModel;
using System.Collections.ObjectModel;

using BookManager.Book;

using Moq;
using BookManager.Box;
using BookManager.Genre;
using BookManager.Position;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;

namespace BookManagerTest.Book
{
    [TestClass]
    public class BookViewModel_Test
    {
        private ObservableCollection<BookViewData> testInputData { get; set; } = new ObservableCollection<BookViewData>()
        {
            // 以下はレイアウト確認用のダミーデータ
            new BookViewData(){Id = Guid.NewGuid(), BookName="新世界より1", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Box="文庫1(エンタメ)"},
            new BookViewData(){Id = Guid.NewGuid(), BookName="新世界より2", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Box="文庫1(エンタメ)"},
            new BookViewData(){Id = Guid.NewGuid(), BookName="新世界より3", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Box="文庫1(エンタメ)"}
        };

        /// <summary>
        /// AddBook() OK
        /// </summary>
        [TestMethod]
        public void AddBook_OK_Test()
        {
            var vm = new BookViewModel();
            vm.BookViewDatas = testInputData;
            var before = vm.BookViewDatas.Count;

            vm.AddBook();

            Assert.AreEqual(before + 1, vm.BookViewDatas.Count);
        }

        [TestMethod]
        public void SaveRead_Test()
        {
            // Ararnge
            var vm = new BookViewModel(bookModel: CreateMock());
            vm.BookViewDatas = testInputData;

            // Action
            vm.SaveBook();
            vm.ReadBook();

            for (int i = 0; i < vm.BookViewDatas.Count; i++)
            {
                // まあ全部はみなくていいでしょ...
                Assert.AreEqual(testInputData[i].Id, vm.BookViewDatas[i].Id);
            }
        }

        [TestMethod]
        public void Save_Test()
        {
            var updateData = new ObservableCollection<BookViewData>(testInputData);
            updateData[1].Operation = OPERATION.UPDATE.ToString();
            updateData[1].Box = "本棚(大)";
            updateData[2].Operation = OPERATION.DELETE.ToString();
            
            var mock = new Mock<IBookModel>();
            var willReturn = new List<BookData>()
            {
                new BookData(){Id = Guid.NewGuid(), BookName="新世界より1", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Box="文庫1(エンタメ)"},
                new BookData(){Id = Guid.NewGuid(), BookName="新世界より2", Auther="貴志祐介", Genre="小説", Position="本棚(大)", Box="文庫1(エンタメ)"},
            };
            mock.Setup(x => x.Read()).Returns(willReturn);

            var vm = new BookViewModel(mock.Object);
            vm.BookViewDatas = testInputData;
            vm.SaveBook();

            vm.BookViewDatas = updateData;
            vm.SaveBook();
            vm.ReadBook();

            for (int i = 0; i < vm.BookViewDatas.Count; i++)
            {
                Assert.AreEqual(updateData[i].Id, vm.BookViewDatas[i].Id);
            }
        }

        private IBookModel CreateMock()
        {
            // testInputDataを元に作られるはずのデータ
            var modelDatas = from book in testInputData
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.BookName };

            // mockは↑を貰い、↑を返す
            var moqModel = new Mock<IBookModel>();
            moqModel.Setup(x => x.Read()).Returns(modelDatas.ToList());

            return moqModel.Object;
        }

        [TestMethod]
        public void PulldownUpdate_Test()
        {
            // Arrange
            var boxList = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "文庫2(教養)"},
            };
            var boxMock = new Mock<IBoxModel>();
            boxMock.Setup(x => x.Read()).Returns(boxList);

            var genreList = new List<GenreData>()
            {
                new GenreData(){GenreName = "小説"},
                new GenreData(){GenreName = "歴史"}
            };
            var genreMock = new Mock<IGenreModel>();
            genreMock.Setup(x => x.Read()).Returns(genreList);

            var positionList = new List<PositionData>() {
                new PositionData(){Position = "所属段ボール"},
                new PositionData(){Position = "本棚(小)"}
            };
            var positionMock = new Mock<IPositionModel>();
            positionMock.Setup(x => x.Read()).Returns(positionList);

            // Action
            var vm = new BookViewModel(
                boxModel: boxMock.Object,
                genreModel: genreMock.Object,
                positionModel: positionMock.Object);
            vm.UpdatePulldown();

            // Assert
            Assert.AreEqual(boxList.Count, vm.BoxChoces.Count);
            for (var i = 0; i < boxList.Count; i++)
            {
                Assert.AreEqual(boxList[i].BoxName, vm.BoxChoces[i]);
            }

            Assert.AreEqual(genreList.Count, vm.GenreChoces.Count);
            for (var i = 0; i < genreList.Count; i++)
            {
                Assert.AreEqual(genreList[i].GenreName, vm.GenreChoces[i]);
            }

            Assert.AreEqual(positionList.Count, vm.PositionChoces.Count);
            for (var i = 0; i < positionList.Count; i++)
            {
                Assert.AreEqual(positionList[i].Position, vm.PositionChoces[i]);
            }
        }
    }
}
