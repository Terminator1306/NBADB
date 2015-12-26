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
using System.Data;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for TeamRank.xaml
    /// </summary>
    public partial class TeamRank : Window
    {
        private int s;
        public TeamRank()
        {
            InitializeComponent();
            initcomobox();
        }

        private void season_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            s = (int)season.SelectedValue;
            initrank();
        }

        private void initrank()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            conn.Open();
            DataSet set = new DataSet();
            string sql = String.Format("call getRank('{0}')", s);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "rank");
            using (DataTable table = set.Tables["rank"])
            {
                table.Columns["rowno"].ColumnName = "排名";
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["gamenum"].ColumnName = "场数";
                table.Columns["win"].ColumnName = "胜场";
                table.Columns["lose"].ColumnName = "负场";
                table.Columns["rate"].ColumnName = "胜率";
            }
            rank.DataContext = set;
            rank.ItemsSource = set.Tables["rank"].DefaultView;
            rank.IsReadOnly = true;
        }

        private void initcomobox()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter("select seasonid from season", conn);
            adapter.Fill(set, "season");
            season.DataContext = set;
            season.ItemsSource = set.Tables["season"].DefaultView;
            season.DisplayMemberPath = "seasonid";
            season.SelectedValuePath = "seasonid";
            season.SelectedIndex = 0;
        }
    }
}
