using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BookManager.Book.BookViewModel;
using System.Collections.ObjectModel;

using BookManager.Book;

namespace BookManagerTest.Book
{
    [TestClass]
    public class BookViewModel_Test
    {
        private ObservableCollection<BookViewData> testInputData { get; set; } = new ObservableCollection<BookViewData>()
        {
            // 以下はレイアウト確認用のダミーデータ
            new BookViewData(){Id = Guid.NewGuid(), BookName="新世界より1", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Carbonboard="文庫1(エンタメ)"},
            new BookViewData(){Id = Guid.NewGuid(), BookName="新世界より2", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Carbonboard="文庫1(エンタメ)"},
            new BookViewData(){Id = Guid.NewGuid(), BookName="新世界より3", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Carbonboard="文庫1(エンタメ)"}
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
    }
}
