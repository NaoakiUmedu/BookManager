using BookManager.Box;
using BookManagerTest.TestDb;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerTest.Box
{
    [TestClass]
    public class SqliteBoxDataAccess_Test
    {
        [TestMethod]
        public void SelectAndInsert_Test()
        {
            TestDbTableCreateDelete.CreateTable();

            // Arrange
            var testData = new List<BoxData>() { 
                new BoxData(){BoxName = "文庫1(エンタメ)'"},
                new BoxData(){BoxName = "文庫2(教養)'"},
                new BoxData(){BoxName = "漫画1'"},
            };

            // Act
            var da = new SqliteBoxDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            foreach(var data in testData)
            {
                da.InsertBox(data);
            }
            var selected = da.SelectAllBox();

            // Assert
            // 比較を簡略化するのためソート
            testData.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            selected.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            for(var i = 0; i < selected.Count; i++)
            {
                Assert.AreEqual(testData[i].BoxName, selected[i].BoxName);
            }

            TestDbTableCreateDelete.DropTable();
        }

        [TestMethod]
        public void Delete_Test()
        {
            TestDbTableCreateDelete.CreateTable();

            // Arrange
            var testData = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "文庫2(教養)'"},
                new BoxData(){BoxName = "漫画1"},
            };
            var da = new SqliteBoxDataAccess(TestDbTableCreateDelete.TEST_DB_FILE_PATH);
            foreach (var data in testData)
            {
                da.InsertBox(data);
            }

            // Act
            da.DeleteBox(new BoxData() { BoxName = "文庫2(教養)'" });
            var selected = da.SelectAllBox();

            // Assert
            var expectData = new List<BoxData>() {
                new BoxData(){BoxName = "文庫1(エンタメ)"},
                new BoxData(){BoxName = "漫画1"},
            };
            // 比較を簡略化するのためソート
            expectData.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            selected.Sort((s, t) => s.BoxName.CompareTo(t.BoxName));
            for (var i = 0; i < selected.Count; i++)
            {
                Assert.AreEqual(expectData[i].BoxName, selected[i].BoxName);
            }

            TestDbTableCreateDelete.DropTable();
        }
    }
}
