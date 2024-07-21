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

namespace BookManager.Box
{
    /// <summary>
    /// BoxWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BoxWindow : Window
    {
        public BoxWindow()
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
            var vm = this.DataContext as BoxViewModel;
            vm?.AddBox();
        }

        /// <summary>
        /// 削除ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedBoxNames = new List<string>();
            foreach (var item in BoxDataGrid.SelectedItems)
            {
                var si = item as BoxViewModel.BoxViewData;
                if (si != null)
                {
                    selectedBoxNames.Add(si.BoxName);
                }
            }
            foreach (var id in selectedBoxNames)
            {
                var vm = this.DataContext as BoxViewModel;
                vm?.DeleteBox(id);
            }
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BoxViewModel;
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
            var vm = this.DataContext as BoxViewModel;
            vm?.Read();
        }
    }
}
