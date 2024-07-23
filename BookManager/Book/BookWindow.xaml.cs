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

namespace BookManager.Book
{
    /// <summary>
    /// BookWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BookWindow : Window
    {
        public BookWindow()
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
            var vm = this.DataContext as BookViewModel;
            vm?.AddBook();
        }

        /// <summary>
        /// 削除ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedBookId = new List<Guid>();
            foreach(var item in DataGrid_Book.SelectedItems)
            {
                var si = item as BookViewModel.BookViewData;
                if (si != null)
                {
                    selectedBookId.Add(si.Id);
                }
            }
            foreach(var id in selectedBookId)
            {
                var vm = (BookViewModel)this.DataContext;
                vm?.DeleteBook(id);
            }
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BookViewModel;
            vm?.SaveBook();
        }

        /// <summary>
        /// ウィンドウロードイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BookViewModel;
            vm?.ReadBook();
            vm?.UpdatePulldown();
        }
    }
}
