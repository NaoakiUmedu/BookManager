using BookManager.Genre;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManager.Book.BookViewModel;

namespace BookManagerTest.Genre
{
    [TestClass]
    public class GenreViewModel_Test
    {
        [TestMethod]
        public void AddGenre_Test()
        {
            var vm = new GenreViewModel();
            var pre = vm.GenreViewDatas.Count;

            vm.AddGenre();

            Assert.AreEqual(pre + 1, vm.GenreViewDatas.Count);
        }

        [TestMethod]
        public void DeleteGenre_Test()
        {
            var vm = new GenreViewModel();
            vm.AddGenre();
            vm.GenreViewDatas[0].GenreName = "歴史";
            vm.AddGenre();
            vm.GenreViewDatas[1].GenreName = "小説";

            vm.DeleteGenre("歴史");
            vm.DeleteGenre("コンピュータ");   // 別にへんなの入れてもなにもおこらんよという試験(カバレッジ満たしたいのもある)

            Assert.IsFalse(vm.GenreViewDatas.Any(x => x.GenreName == "歴史"));
        }


        [TestMethod]
        public void SimpleSaveAndRead_Test()
        {
            ObservableCollection<GenreViewModel.GenreViewData> firstData = [
                new GenreViewModel.GenreViewData(){ GenreName = "歴史" },
                new GenreViewModel.GenreViewData(){ GenreName = "小説" },
                new GenreViewModel.GenreViewData(){ GenreName = "自然科学"}
            ];

            // Arrange
            var stubModel = new GenreModelStub();

            var vm = new GenreViewModel(stubModel);
            SetGenreViewDatas(ref vm, firstData);

            // Action
            vm.Save();
            vm.Read();

            // Assert
            for (int i = 0; i < vm.GenreViewDatas.Count; i++)
            {
                Assert.AreEqual(firstData[i].GenreName, vm.GenreViewDatas[i].GenreName);
            }
        }

        public void UpdateSaveAndRead_Test()
        {
            ObservableCollection<GenreViewModel.GenreViewData> firstData = [
                new GenreViewModel.GenreViewData(){ GenreName = "歴史" },
                new GenreViewModel.GenreViewData(){ GenreName = "小説" },
                new GenreViewModel.GenreViewData(){ GenreName = "自然科学"}
            ];

            ObservableCollection<GenreViewModel.GenreViewData> updatedData = [
                new GenreViewModel.GenreViewData(){ GenreName = "歴史" },
                new GenreViewModel.GenreViewData(){ GenreName = "小説" },
                new GenreViewModel.GenreViewData(){ GenreName = "音楽"}
            ];

            // Arrange
            var stubModel = new GenreModelStub();

            var vm = new GenreViewModel(stubModel);
            SetGenreViewDatas(ref vm, firstData);

            // Action
            vm.Save();
            vm.Read();
            SetGenreViewDatas(ref vm, updatedData);
            vm.Save();
            vm.Read();

            // Assert
            for (int i = 0; i < vm.GenreViewDatas.Count; i++)
            {
                Assert.AreEqual(updatedData[i].GenreName, vm.GenreViewDatas[i].GenreName);
            }
        }

        /// <summary>
        /// GenreViewDatasを設定する(ClearしてConcatする)
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <param name="data">データ</param>
        private void SetGenreViewDatas(ref GenreViewModel vm, ObservableCollection<GenreViewModel.GenreViewData> data)
        {
            vm.GenreViewDatas.Clear();
            vm.GenreViewDatas.Concat(data);
        }

        /// <summary>
        /// 差分を取るテストがあるのでもう作っちゃう
        /// </summary>
        private class GenreModelStub : IGenreModel
        {
            List<GenreData> innerData = [];
            /// <summary>
            /// ジャンルを挿入
            /// </summary>
            /// <param name=""></param>
            public void Insert(List<GenreData> data)
            {
                this.innerData.Concat(data);
            }
            /// <summary>
            /// 全件読み込み
            /// </summary>
            /// <returns>データ</returns>
            public List<GenreData> Read()
            {
                return innerData;
            }
            /// <summary>
            /// 指定したジャンルを削除
            /// </summary>
            /// <param name="data"></param>
            public void Delete(List<GenreData> data)
            {
                foreach (var datum in data)
                {
                    for (var i = 0; i < innerData.Count; i++)
                    {
                        if (datum.GenreName == innerData[i].GenreName)
                        {
                            innerData.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
