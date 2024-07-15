using BookManager.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookManager.Genre
{
    /// <summary>
    /// GenreWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class GenreWindow : Window
    {
        public GenreWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 追加ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as GenreViewModel;
            vm?.AddGenre();
        }

        /// <summary>
        /// 削除ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenreNames = new List<string>();
            foreach (var item in GenreDataGrid.SelectedItems)
            {
                var si = item as GenreViewModel.GenreViewData;
                if (si != null)
                {
                    selectedGenreNames.Add(si.GenreName);
                }
            }
            foreach (var id in selectedGenreNames)
            {
                var vm = this.DataContext as GenreViewModel;
                vm?.DeleteGenre(id);
            }
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as GenreViewModel;
            vm?.Save();
            vm?.Read();
        }

        /// <summary>
        /// ウィンドウロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as GenreViewModel;
            vm?.Read();
        }
    }
}
