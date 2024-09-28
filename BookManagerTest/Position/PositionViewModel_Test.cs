using BookManager.Position;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerTest.Position
{
    [TestClass]
    public class PositionViewModel_Test
    {
        [TestMethod]
        public void AddPosition_Test()
        {
            var vm = new PositionViewModel();
            var pre = vm.PositionViewDatas.Count;

            vm.AddPosition();

            Assert.AreEqual(pre + 1, vm.PositionViewDatas.Count);
        }

        [TestMethod]
        public void DeletePosition_Test()
        {
            var vm = new PositionViewModel();
            vm.AddPosition();
            vm.PositionViewDatas[0].Position = "本棚(小)";
            vm.AddPosition();
            vm.PositionViewDatas[1].Position = "本棚(大)";

            vm.DeletePosition("本棚(小)");
            vm.DeletePosition("所属段ボール");   // 別にへんなの入れてもなにもおこらんよという試験(カバレッジ満たしたいのもある)

            Assert.IsFalse(vm.PositionViewDatas.Any(x => x.Position == "本棚(小)"));
        }


        [TestMethod]
        public void SimpleSaveAndRead_Test()
        {
            ObservableCollection<PositionViewModel.PositionViewData> firstData = [
                new PositionViewModel.PositionViewData(){ Position = "本棚(小)" },
                new PositionViewModel.PositionViewData(){ Position = "本棚(大)" },
                new PositionViewModel.PositionViewData(){ Position = "プラケース1"}
            ];

            // Arrange
            var stubModel = new PositionModelStub();

            var vm = new PositionViewModel(stubModel);
            SetPositionViewDatas(ref vm, firstData);

            // Action
            vm.Save();
            vm.Read();

            // Assert
            for (int i = 0; i < vm.PositionViewDatas.Count; i++)
            {
                Assert.AreEqual(firstData[i].Position, vm.PositionViewDatas[i].Position);
            }
        }

        public void UpdateSaveAndRead_Test()
        {
            ObservableCollection<PositionViewModel.PositionViewData> firstData = [
                new PositionViewModel.PositionViewData(){ Position = "本棚(小)" },
                new PositionViewModel.PositionViewData(){ Position = "本棚(大)" },
                new PositionViewModel.PositionViewData(){ Position = "プラケース1"}
            ];

            ObservableCollection<PositionViewModel.PositionViewData> updatedData = [
                new PositionViewModel.PositionViewData(){ Position = "本棚(小)" },
                new PositionViewModel.PositionViewData(){ Position = "本棚(大)" },
                new PositionViewModel.PositionViewData(){ Position = "音楽"}
            ];

            // Arrange
            var stubModel = new PositionModelStub();

            var vm = new PositionViewModel(stubModel);
            SetPositionViewDatas(ref vm, firstData);

            // Action
            vm.Save();
            vm.Read();
            SetPositionViewDatas(ref vm, updatedData);
            vm.Save();
            vm.Read();

            // Assert
            for (int i = 0; i < vm.PositionViewDatas.Count; i++)
            {
                Assert.AreEqual(updatedData[i].Position, vm.PositionViewDatas[i].Position);
            }
        }

        /// <summary>
        /// 前回Readしたデータが今回Updateしたと勘違いしてしまう事象に対応
        /// </summary>
        [TestMethod]
        public void DuplicateRead_Test()
        {
            ObservableCollection<PositionViewModel.PositionViewData> firstData = [
                new PositionViewModel.PositionViewData(){ Position = "本棚(小)" },
                new PositionViewModel.PositionViewData(){ Position = "本棚(大)" },
                new PositionViewModel.PositionViewData(){ Position = "プラケース1"}
            ];
            ObservableCollection<PositionViewModel.PositionViewData> secondData = [
                new PositionViewModel.PositionViewData(){ Position = "本棚(小)" },
                new PositionViewModel.PositionViewData(){ Position = "本棚(大)" },
                new PositionViewModel.PositionViewData(){ Position = "プラケース1"},
                new PositionViewModel.PositionViewData(){ Position = "音楽"}
            ];

            // Arrange
            var stubModel = new PositionModelStub();

            var vm = new PositionViewModel(stubModel);

            // Action
            SetPositionViewDatas(ref vm, firstData);
            vm.Save();
            vm.Read();
            SetPositionViewDatas(ref vm, secondData);
            vm.Save();
            vm.Read();

            // Assert
            // Save()で例外を吐かないこと！
        }

        /// <summary>
        /// PositionViewDatasを設定する(ClearしてConcatする)
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <param name="data">データ</param>
        private void SetPositionViewDatas(ref PositionViewModel vm, ObservableCollection<PositionViewModel.PositionViewData> data)
        {
            // Concatではだめ
            vm.PositionViewDatas.Clear();
            foreach (var datum in data)
            {
                vm.PositionViewDatas.Add(datum);
            }
        }

        /// <summary>
        /// 差分を取るテストがあるのでもう作っちゃう
        /// </summary>
        private class PositionModelStub : IPositionUsecase
        {
            List<PositionData> innerData = [];
            /// <summary>
            /// ジャンルを挿入
            /// </summary>
            /// <param name=""></param>
            public void Insert(List<PositionData> data)
            {
                foreach (var datum in data)
                {
                    if (innerData.Exists(x => x.Position == datum.Position))
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
            public List<PositionData> Read()
            {
                return innerData;
            }
            /// <summary>
            /// 指定したジャンルを削除
            /// </summary>
            /// <param name="data"></param>
            public void Delete(List<PositionData> data)
            {
                foreach (var datum in data)
                {
                    for (var i = 0; i < innerData.Count; i++)
                    {
                        if (datum.Position == innerData[i].Position)
                        {
                            innerData.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
