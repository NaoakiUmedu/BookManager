using BookManager.Position;
using BookManagerTest.TestDb;

namespace BookManagerTest.Position
{
    [TestClass]
    public class SqlitePositionDataAccess_Test
    {
        [TestMethod]
        public void SelectAndInsert_Test()
        {
            TestDbTableCreateDelete.CreateTable();

            // Arrange
            var testData = new List<PositionData>() {
                new PositionData(){Position = "本棚(小)"},
                new PositionData(){Position = "本棚(大)"},
                new PositionData(){Position = "所属段ボール"},
            };

            // Act
            var da = new SqlitePositionDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            foreach (var data in testData)
            {
                da.InsertPosition(data);
            }
            var selected = da.SelectAllPosition();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.Position.CompareTo(t.Position));
            selected.Sort((s, t) => s.Position.CompareTo(t.Position));
            for (var i = 0; i < selected.Count; i++)
            {
                Assert.AreEqual(testData[i].Position, selected[i].Position);
            }

            TestDbTableCreateDelete.DropTable();
        }

        [TestMethod]
        public void Delete_Test()
        {
            TestDbTableCreateDelete.CreateTable();

            // Arrange
            var testData = new List<PositionData>() {
                new PositionData(){Position = "本棚(小)"},
                new PositionData(){Position = "本棚(大)'"},
                new PositionData(){Position = "所属段ボール"},
            };
            var da = new SqlitePositionDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            foreach (var data in testData)
            {
                da.InsertPosition(data);
            }

            // Act
            da.DeletePosition(new PositionData() { Position = "本棚(大)'" });
            var selected = da.SelectAllPosition();

            // Assert
            var expectData = new List<PositionData>() {
                new PositionData(){Position = "本棚(小)"},
                new PositionData(){Position = "所属段ボール"},
            };
            // 比較を簡略化するのためソート
            expectData.Sort((s, t) => s.Position.CompareTo(t.Position));
            selected.Sort((s, t) => s.Position.CompareTo(t.Position));
            for (var i = 0; i < selected.Count; i++)
            {
                Assert.AreEqual(expectData[i].Position, selected[i].Position);
            }

            TestDbTableCreateDelete.DropTable();
        }
    }
}
