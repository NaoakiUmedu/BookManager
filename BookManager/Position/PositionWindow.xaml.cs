using BookManager.Position;
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

namespace BookManager.Position
{
    /// <summary>
    /// PositionWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PositionWindow : Window
    {
        public PositionWindow()
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
            var vm = this.DataContext as PositionViewModel;
            vm?.AddPosition();
        }

        /// <summary>
        /// 削除ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedPositionNames = new List<string>();
            foreach (var item in PositionDataGrid.SelectedItems)
            {
                var si = item as PositionViewModel.PositionViewData;
                if (si != null)
                {
                    selectedPositionNames.Add(si.Position);
                }
            }
            foreach (var id in selectedPositionNames)
            {
                var vm = this.DataContext as PositionViewModel;
                vm?.DeletePosition(id);
            }
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as PositionViewModel;
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
            var vm = this.DataContext as PositionViewModel;
            vm?.Read();
        }
    }
}
