using BookManager.Box;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManager.Book.BookViewModel;

namespace BookManagerTest.Box
{
    [TestClass]
    public class BoxViewModel_Test
    {
        [TestMethod]
        public void AddBox_Test()
        {
            var vm = new BoxViewModel();
            var pre = vm.BoxViewDatas.Count;

            vm.AddBox();

            Assert.AreEqual(pre + 1, vm.BoxViewDatas.Count);
        }

        [TestMethod]
        public void DeleteBox_Test()
        {
            var vm = new BoxViewModel();
            vm.AddBox();
            vm.BoxViewDatas[0].BoxName = "文庫1(エンタメ)";
            vm.AddBox();
            vm.BoxViewDatas[1].BoxName = "文庫2(教養)";

            vm.DeleteBox("文庫1(エンタメ)");
            vm.DeleteBox("コンピュータ");   // 別にへんなの入れてもなにもおこらんよという試験(カバレッジ満たしたいのもある)

            Assert.IsFalse(vm.BoxViewDatas.Any(x => x.BoxName == "文庫1(エンタメ)"));
        }


        [TestMethod]
        public void SimpleSaveAndRead_Test()
        {
            ObservableCollection<BoxViewModel.BoxViewData> firstData = [
                new BoxViewModel.BoxViewData(){ BoxName = "文庫1(エンタメ)" },
                new BoxViewModel.BoxViewData(){ BoxName = "文庫2(教養)" },
                new BoxViewModel.BoxViewData(){ BoxName = "自然科学"}
            ];

            // Arrange
            var stubModel = new BoxModelStub();

            var vm = new BoxViewModel(stubModel);
            SetBoxViewDatas(ref vm, firstData);

            // Action
            vm.Save();
            vm.Read();

            // Assert
            for (int i = 0; i < vm.BoxViewDatas.Count; i++)
            {
                Assert.AreEqual(firstData[i].BoxName, vm.BoxViewDatas[i].BoxName);
            }
        }

        public void UpdateSaveAndRead_Test()
        {
            ObservableCollection<BoxViewModel.BoxViewData> firstData = [
                new BoxViewModel.BoxViewData(){ BoxName = "文庫1(エンタメ)" },
                new BoxViewModel.BoxViewData(){ BoxName = "文庫2(教養)" },
                new BoxViewModel.BoxViewData(){ BoxName = "自然科学"}
            ];

            ObservableCollection<BoxViewModel.BoxViewData> updatedData = [
                new BoxViewModel.BoxViewData(){ BoxName = "文庫1(エンタメ)" },
                new BoxViewModel.BoxViewData(){ BoxName = "文庫2(教養)" },
                new BoxViewModel.BoxViewData(){ BoxName = "漫画1"}
            ];

            // Arrange
            var stubModel = new BoxModelStub();

            var vm = new BoxViewModel(stubModel);
            SetBoxViewDatas(ref vm, firstData);

            // Action
            vm.Save();
            vm.Read();
            SetBoxViewDatas(ref vm, updatedData);
            vm.Save();
            vm.Read();

            // Assert
            for (int i = 0; i < vm.BoxViewDatas.Count; i++)
            {
                Assert.AreEqual(updatedData[i].BoxName, vm.BoxViewDatas[i].BoxName);
            }
        }

        /// <summary>
        /// 前回Readしたデータが今回Updateしたと勘違いしてしまう事象に対応
        /// </summary>
        [TestMethod]
        public void DuplicateRead_Test()
        {
            ObservableCollection<BoxViewModel.BoxViewData> firstData = [
                new BoxViewModel.BoxViewData(){ BoxName = "文庫1(エンタメ)" },
                new BoxViewModel.BoxViewData(){ BoxName = "文庫2(教養)" },
                new BoxViewModel.BoxViewData(){ BoxName = "自然科学"}
            ];
            ObservableCollection<BoxViewModel.BoxViewData> secondData = [
                new BoxViewModel.BoxViewData(){ BoxName = "文庫1(エンタメ)" },
                new BoxViewModel.BoxViewData(){ BoxName = "文庫2(教養)" },
                new BoxViewModel.BoxViewData(){ BoxName = "自然科学"},
                new BoxViewModel.BoxViewData(){ BoxName = "漫画1"}
            ];

            // Arrange
            var stubModel = new BoxModelStub();

            var vm = new BoxViewModel(stubModel);

            // Action
            SetBoxViewDatas(ref vm, firstData);
            vm.Save();
            vm.Read();
            SetBoxViewDatas(ref vm, secondData);
            vm.Save();
            vm.Read();

            // Assert
            // Save()で例外を吐かないこと！
        }

        /// <summary>
        /// BoxViewDatasを設定する(ClearしてConcatする)
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <param name="data">データ</param>
        private void SetBoxViewDatas(ref BoxViewModel vm, ObservableCollection<BoxViewModel.BoxViewData> data)
        {
            // Concatではだめ
            vm.BoxViewDatas.Clear();
            foreach(var datum in data)
            {
                vm.BoxViewDatas.Add(datum);
            }
        }

        /// <summary>
        /// 差分を取るテストがあるのでもう作っちゃう
        /// </summary>
        private class BoxModelStub : IBoxModel
        {
            List<BoxData> innerData = [];
            /// <summary>
            /// 段ボールを挿入
            /// </summary>
            /// <param name=""></param>
            public void Insert(List<BoxData> data)
            {
                foreach(var datum in data)
                {
                    if(innerData.Exists(x => x.BoxName == datum.BoxName))
                    {
                        throw new Exception("同じデータをinsertするのはだめ！");
                    }
                    innerData.Add(datum);
                }
            }
            /// <summary>
            /// 全件読み込み
            /// </summary>
            /// <returns>データ</returns>
            public List<BoxData> Read()
            {
                return innerData;
            }
            /// <summary>
            /// 指定した段ボールを削除
            /// </summary>
            /// <param name="data"></param>
            public void Delete(List<BoxData> data)
            {
                foreach (var datum in data)
                {
                    for (var i = 0; i < innerData.Count; i++)
                    {
                        if (datum.BoxName == innerData[i].BoxName)
                        {
                            innerData.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
