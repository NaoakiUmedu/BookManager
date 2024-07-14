using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BookManager.Book.BookViewModel;
using System.Collections.ObjectModel;

using BookManager.Book;

using Moq;

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

        /// <summary>
        /// DeleteBook_OK
        /// </summary>
        [TestMethod]
        public void DeleteBook_OK_Test()
        {
            var vm = new BookViewModel();
            vm.BookViewDatas = testInputData;
            var deleteData = testInputData[1];

            vm.DeleteBook(deleteData.Id);

            foreach (var book in vm.BookViewDatas)
            {
                Assert.AreNotEqual(deleteData.Id, book.Id);
            }
        }

        /// <summary>
        /// DeleteBook_NG(存在しないID)
        /// </summary>
        [TestMethod]
        public void DeleteBook_NG_NOWHERE_ID_Test()
        {
            var vm = new BookViewModel();
            vm.BookViewDatas = testInputData;
            var deleteId = Guid.NewGuid();

            vm.DeleteBook(deleteId);

            foreach (var book in vm.BookViewDatas)
            {
                Assert.AreNotEqual(deleteId, book.Id);
            }
        }

        [TestMethod]
        public void SaveRead_Test()
        {
            // Ararnge
            var vm = new BookViewModel(model: CreateMock());
            vm.BookViewDatas = testInputData;

            // Action
            vm.SaveBook();
            vm.ReadBook();

            for(int i = 0; i < vm.BookViewDatas.Count; i++)
            {
                // まあ全部はみなくていいでしょ...
                Assert.AreEqual(testInputData[i].Id, vm.BookViewDatas[i].Id);
            }
        }

        private IBookModelInterface CreateMock()
        {
            // testInputDataを元に作られるはずのデータ
            var modelDatas = from book in testInputData
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.BookName };

            // mockは↑を貰い、↑を返す
            var moqModel = new Mock<IBookModelInterface>();
            moqModel.Setup(x => x.Read()).Returns(modelDatas.ToList());

            return moqModel.Object;
        }
    }
}
