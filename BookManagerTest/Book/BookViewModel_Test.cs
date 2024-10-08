﻿using static BookManager.Book.BookViewModel;
using System.Collections.ObjectModel;

using BookManager.Book;

using Moq;
using BookManager.Box;
using BookManager.Genre;
using BookManager.Position;

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
            var box = new Mock<IBoxUsecase>();
            box.Setup(x => x.Read()).Returns(new List<BoxData>() { new BoxData { BoxName = "新書1" } });
            box.Setup(x => x.Insert(new List<BoxData>() { new BoxData { BoxName = "自然科学1" } })).Verifiable();

            var genre = new Mock<IGenreUsecase>();
            genre.Setup(x => x.Read()).Returns(new List<GenreData>() { new GenreData { GenreName = "自然科学" } });
            genre.Setup(x => x.Insert(new List<GenreData>() { new GenreData { GenreName = "歴史" } })).Verifiable();

            var position = new Mock<IPositionUsecase>();
            position.Setup(x => x.Read()).Returns(new List<PositionData>() { new PositionData { Position = "本棚(小)" } });
            position.Setup(x => x.Insert(new List<PositionData>() { new PositionData { Position = "本棚(大)" } })).Verifiable();

            // Ararnge
            var vm = new BookViewModel(bookUsecaseImpl: CreateMock(), boxUsecaseImpl:box.Object, genreUsecaseImpl:genre.Object, positionUsecaseImpl:position.Object);
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
            
            var willReturn = new List<BookData>()
            {
                new BookData(){Id = Guid.NewGuid(), BookName="新世界より1", Auther="貴志祐介", Genre="小説", Position="所属段ボール", Box="文庫1(エンタメ)"},
                new BookData(){Id = Guid.NewGuid(), BookName="新世界より2", Auther="貴志祐介", Genre="小説", Position="本棚(大)", Box="文庫1(エンタメ)"},
            };
            var mock = new Mock<IBookUsecase>();
            mock.Setup(x => x.Read()).Returns(willReturn);

            var box = new Mock<IBoxUsecase>();
            box.Setup(x => x.Read()).Returns(new List<BoxData>() { new BoxData { BoxName = "新書1" } });
            box.Setup(x => x.Insert(new List<BoxData>() { new BoxData { BoxName = "自然科学1" } })).Verifiable();

            var genre = new Mock<IGenreUsecase>();
            genre.Setup(x => x.Read()).Returns(new List<GenreData>() { new GenreData { GenreName = "自然科学" } });
            genre.Setup(x => x.Insert(new List<GenreData>() { new GenreData { GenreName = "歴史" } })).Verifiable();

            var position = new Mock<IPositionUsecase>();
            position.Setup(x => x.Read()).Returns(new List<PositionData>() { new PositionData { Position = "本棚(小)" } });
            position.Setup(x => x.Insert(new List<PositionData>() { new PositionData { Position = "本棚(大)" } })).Verifiable();

            var vm = new BookViewModel(mock.Object, box.Object, genre.Object, position.Object);
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

        /// <summary>
        /// 保存押したときに外部キーが足りなかったらInsertしてあげるテスト
        /// </summary>
        [TestMethod]
        public void SaveWithReferenceKey_Test()
        {
            var inputData = new ObservableCollection<BookViewData>()
            {
                new BookViewData()
                {
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookViewData()
                {
                    BookName = "ヤマト王権",
                    Auther = "吉村武彦",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookViewData()
                {
                    BookName = "数学再入門",
                    Auther = "長岡亮介",
                    Genre = "自然科学",
                    Position = "本棚(大)",
                    Box = "自然科学1"
                }
            };
            var willReturn = new List<BookData>()
            {
                new BookData()
                {
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookData()
                {
                    BookName = "ヤマト王権",
                    Auther = "吉村武彦",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookData()
                {
                    BookName = "数学再入門",
                    Auther = "長岡亮介",
                    Genre = "自然科学",
                    Position = "本棚(大)",
                    Box = "自然科学1"
                }
            };

            var mock = new Mock<IBookUsecase>();
            mock.Setup(x => x.Read()).Returns(willReturn);

            var box = new Mock<IBoxUsecase>();
            box.Setup(x => x.Read()).Returns(new List<BoxData>() { new BoxData { BoxName = "新書1" } });
            box.Setup(x => x.Insert(new List<BoxData>() { new BoxData { BoxName = "自然科学1" } })).Verifiable();

            var genre = new Mock<IGenreUsecase>();
            genre.Setup(x => x.Read()).Returns(new List<GenreData>() { new GenreData { GenreName = "自然科学" } });
            genre.Setup(x => x.Insert(new List<GenreData>() { new GenreData { GenreName = "歴史" } })).Verifiable();

            var position = new Mock<IPositionUsecase>();
            position.Setup(x => x.Read()).Returns(new List<PositionData>() { new PositionData { Position = "本棚(小)" } });
            position.Setup(x => x.Insert(new List<PositionData>() { new PositionData { Position = "本棚(大)" } })).Verifiable();

            var vm = new BookViewModel(mock.Object, box.Object, genre.Object, position.Object);
            vm.BookViewDatas = inputData;
            vm.SaveBook();
            vm.ReadBook();

            for (int i = 0; i < vm.BookViewDatas.Count; i++)
            {
                Assert.AreEqual(willReturn[i].Id, vm.BookViewDatas[i].Id);
            }

            box.Verify();
            genre.Verify();
            position.Verify();
        }

        private IBookUsecase CreateMock()
        {
            // testInputDataを元に作られるはずのデータ
            var modelDatas = from book in testInputData
                             select new BookData() { Id = book.Id, BookName = book.BookName, Auther = book.Auther, Genre = book.Genre, Position = book.Position, Box = book.BookName };

            // mockは↑を貰い、↑を返す
            var moqModel = new Mock<IBookUsecase>();
            moqModel.Setup(x => x.Read()).Returns(modelDatas.ToList());

            return moqModel.Object;
        }

        /// <summary>
        /// プルダウンの選択肢を入れるテスト
        /// </summary>
        [TestMethod]
        public void PulldownUpdate_Test()
        {
            // Arrange
            var boxList = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "文庫2(教養)"},
            };
            var boxMock = new Mock<IBoxUsecase>();
            boxMock.Setup(x => x.Read()).Returns(boxList);

            var genreList = new List<GenreData>()
            {
                new GenreData(){GenreName = "小説"},
                new GenreData(){GenreName = "歴史"}
            };
            var genreMock = new Mock<IGenreUsecase>();
            genreMock.Setup(x => x.Read()).Returns(genreList);

            var positionList = new List<PositionData>() {
                new PositionData(){Position = "所属段ボール"},
                new PositionData(){Position = "本棚(小)"}
            };
            var positionMock = new Mock<IPositionUsecase>();
            positionMock.Setup(x => x.Read()).Returns(positionList);

            // Action
            var vm = new BookViewModel(
                boxUsecaseImpl: boxMock.Object,
                genreUsecaseImpl: genreMock.Object,
                positionUsecaseImpl: positionMock.Object);
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

        /// <summary>
        /// インポートのテスト
        /// </summary>
        [TestMethod]
        public void Import_Test()
        {
            var filePath = @"C:\Users\anija\Desktop\codes\BookManager\BookManagerTest\TestDb\Test.tsv";
            var willReturn = new List<BookData>()
            {
                new BookData()
                {
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookData()
                {
                    BookName = "数学再入門",
                    Auther = "長岡亮介",
                    Genre = "自然科学",
                    Position = "本棚(大)",
                    Box = "自然科学1"
                }
            };

            var mock = new Mock<IBookUsecase>();
            mock.Setup(x => x.Import(filePath)).Returns(willReturn).Verifiable();

            var vm = new BookViewModel(bookUsecaseImpl:mock.Object);
            vm.Import(filePath);

            Assert.AreEqual(willReturn.Count, vm.BookViewDatas.Count);
            for(int i = 0; i < willReturn.Count; i++)
            {
                Assert.AreEqual(OPERATION.INSERT.ToString(), vm.BookViewDatas[i].Operation);
                Assert.AreEqual(willReturn[i].BookName, vm.BookViewDatas[i].BookName);
                Assert.AreEqual(willReturn[i].Auther, vm.BookViewDatas[i].Auther);
                Assert.AreEqual(willReturn[i].Genre, vm.BookViewDatas[i].Genre);
                Assert.AreEqual(willReturn[i].Position, vm.BookViewDatas[i].Position);
                Assert.AreEqual(willReturn[i].Box, vm.BookViewDatas[i].Box);
            }

            var genres = (from book in willReturn
                         select book.Genre).ToList();
            genres.Sort();
            var genreChoises = vm.GenreChoces.ToList();
            genreChoises.Sort();
            Assert.AreEqual(genres.Count, genreChoises.Count);
            for (int i = 0; i < willReturn.Count; i++)
            {
                Assert.AreEqual(genres[i], genreChoises[i]);
            }

            var boxes = (from book in willReturn
                          select book.Box).ToList();
            boxes.Sort();
            var boxChoises = vm.BoxChoces.ToList();
            boxChoises.Sort();
            Assert.AreEqual(boxes.Count, boxChoises.Count);
            for (int i = 0; i < boxes.Count; i++)
            {
                Assert.AreEqual(boxes[i], boxChoises[i]);
            }

            var positions = (from book in willReturn
                         select book.Position).ToList();
            positions.Sort();
            var positionChoises = vm.PositionChoces.ToList();
            positionChoises.Sort();
            Assert.AreEqual(positions.Count, positionChoises.Count);
            for (int i = 0; i < positions.Count; i++)
            {
                Assert.AreEqual(positions[i], positionChoises[i]);
            }

            mock.Verify();
        }

        [TestMethod]
        public void Export_Test()
        {
            var filePath = @"C:\Users\anija\Desktop\codes\BookManager\BookManagerTest\TestDb\Test.tsv";
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var willCalled = new List<BookData>()
            {
                new BookData()
                {
                    Id = id1,
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookData()
                {
                    Id = id2,
                    BookName = "数学再入門",
                    Auther = "長岡亮介",
                    Genre = "自然科学",
                    Position = "本棚(大)",
                    Box = "自然科学1"
                }
            };
            var inputData = new ObservableCollection<BookViewData>()
            {
                new BookViewData()
                {
                    Id = id1,
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookViewData()
                {
                    Id = id2,
                    BookName = "数学再入門",
                    Auther = "長岡亮介",
                    Genre = "自然科学",
                    Position = "本棚(大)",
                    Box = "自然科学1"
                }
            };

            var mock = new Mock<IBookUsecase>();
            mock.Setup(x => x.Export(filePath, willCalled)).Verifiable();

            var vm = new BookViewModel(bookUsecaseImpl: mock.Object);
            vm.BookViewDatas = inputData;
            vm.Export(filePath);

            mock.Verify();
        }

        [TestMethod]
        public void Search_Test()
        {
            var inputData = new ObservableCollection<BookViewData>()
            {
                new BookViewData()
                {
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookViewData()
                {
                    BookName = "数学再入門",
                    Auther = "長岡亮介",
                    Genre = "自然科学",
                    Position = "本棚(大)",
                    Box = "自然科学1"
                },
                new BookViewData()
                {
                    BookName = "明治洋食事始め",
                    Auther = "岡田 哲",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "文庫2(教養)"
                }
            };

            var willSearchedData = new ObservableCollection<BookViewData>()
            {
                new BookViewData()
                {
                    BookName = "ある明治人の記録",
                    Auther = "柴五郎",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "新書1"
                },
                new BookViewData()
                {
                    BookName = "明治洋食事始め",
                    Auther = "岡田 哲",
                    Genre = "歴史",
                    Position = "本棚(小)",
                    Box = "文庫2(教養)"
                }
            };

            var vm = new BookViewModel();
            vm.BookViewDatas = inputData;

            vm.Seach("明治");

            Assert.AreEqual(willSearchedData.Count, vm.BookViewDatas.Count);
            for(int i = 0; i < willSearchedData.Count; i++)
            {
                Assert.AreEqual(willSearchedData[i].BookName, vm.BookViewDatas[i].BookName);
                Assert.AreEqual(willSearchedData[i].Auther, vm.BookViewDatas[i].Auther);
                Assert.AreEqual(willSearchedData[i].Genre, vm.BookViewDatas[i].Genre);
                Assert.AreEqual(willSearchedData[i].Position, vm.BookViewDatas[i].Position);
                Assert.AreEqual(willSearchedData[i].Box, vm.BookViewDatas[i].Box);
            }
        }
    }
}
