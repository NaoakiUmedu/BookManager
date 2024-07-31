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

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            var filePath = SelectFile();
            if(filePath != string.Empty)
            {
                var vm = this.DataContext as BookViewModel;
                vm?.Import(filePath);
            }
        }

        /// <summary>
        /// ファイルを選択する
        /// </summary>
        /// <returns>ファイル</returns>
        private string SelectFile()
        {
            string filename = string.Empty;

            
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = "";
            dialog.Filter = "tsv|*.tsv";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                // Open document
                filename = dialog.FileName;
            }

            return filename;
        }
        /// <summary>
        /// 検索ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Seach_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BookViewModel;
            vm?.Seach(TextBox_Search.Text);
        }

        /// <summary>
        /// 検索解除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Seach_Cancel_Click(object sender, RoutedEventArgs e)
        {
            // 単純にReadしちゃう
            var vm = this.DataContext as BookViewModel;
            vm?.ReadBook();
        }
    }
}
