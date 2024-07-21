using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookManager
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 蔵書一覧ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Book_Click(object sender, RoutedEventArgs e)
        {
            new Book.BookWindow().ShowDialog();
        }

        /// <summary>
        /// ジャンルボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Genre_Click(object sender, RoutedEventArgs e)
        {
            new Genre.GenreWindow().ShowDialog();
        }

        /// <summary>
        /// 配置一覧ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Position_Click(object sender, RoutedEventArgs e)
        {
            new Position.PositionWindow().ShowDialog();
        }

        /// <summary>
        /// 段ボール一覧ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Cardboard_Click(object sender, RoutedEventArgs e)
        {
            new Box.BoxWindow().ShowDialog();
        }
    }
}