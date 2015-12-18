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
using MySql.Data.MySqlClient;
namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for GameScheduleWindow.xaml
    /// </summary>
    public partial class GameScheduleWindow : Window
    {
        public GameScheduleWindow()
        {
            InitializeComponent();
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DBHelper dbhelper = new DBHelper("nbadb");
            MySqlConnection con = dbhelper.getCon();
        }
    }
}
