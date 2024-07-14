using BookManager.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerTest.Genre
{
    [TestClass]
    public class GenreViewModel_Test
    {
        [TestMethod]
        public void AddGenre_Test()
        {
            var vm = new GenreViewModel();
            var pre = vm.Genres.Count;

            vm.AddGenre();

            Assert.AreEqual(pre + 1, vm.Genres.Count);
        }

        [TestMethod]
        public void DeleteGenre_Test()
        {
            var vm = new GenreViewModel();
            vm.AddGenre();
            vm.Genres[0].GenreName = "歴史";
            vm.AddGenre();
            vm.Genres[1].GenreName = "小説";

            vm.DeleteGenre("歴史");
            vm.DeleteGenre("コンピュータ");   // 別にへんなの入れてもなにもおこらんよという試験(カバレッジ満たしたいのもある)

            Assert.IsFalse(vm.Genres.Any(x => x.GenreName == "歴史"));
        }

        
    }
}
