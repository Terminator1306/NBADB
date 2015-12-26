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
using MySql.Data;

namespace WpfApplication1
{
    /// <summary>
    /// JudgeInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JudgeInfoWindow : Window
    {
        private DBHelper dbHelper;
        public JudgeInfoWindow()
        {
            InitializeComponent();
            dbHelper = new DBHelper("nbadb2");
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
