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
    /// Interaction logic for TopPlayer.xaml
    /// </summary>

    public partial class TopPlayer : Window
    {
        private string date;
        private int s;
        public TopPlayer()
        {
            InitializeComponent();
            //date =DateTime.Now.ToShortDateString().ToString());
            date = "2015-12-18";
            s = 20152016;
            initcomobox();
            inittoday();
        }

        private void initseason()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();

            string sql = String.Format("select teamname,name,round((score/gamenum),2) as score,playerid from personaldata_season where season='{0}' order by score desc limit 0,5"
                 , s);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "s_score");

            sql = String.Format("select teamname,name,round(((frontreb+backreb)/gamenum),2) as reb,playerid  from personaldata_season where season='{0}' order by reb desc limit 0,5"
                 , s);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "s_reb");

            sql = String.Format("select teamname,name,round((assist/gamenum),2) as assist,playerid  from personaldata_season where season='{0}' order by assist desc limit 0,5"
                 , s);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "s_assist");

            sql = String.Format("select teamname,name,round((steal/gamenum),2) as steal,playerid  from personaldata_season where season='{0}' order by steal desc limit 0,5"
                ,s);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "s_steal");

            sql = String.Format("select teamname,name,round((blockshot/gamenum),2) as blockshot,playerid  from personaldata_season where season='{0}' order by blockshot desc limit 0,5"
                ,s);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "s_block");

            using (DataTable table = set.Tables["s_score"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["score"].ColumnName = "场均得分";
            }
            s_score.DataContext = set;
            s_score.ItemsSource = set.Tables["s_score"].DefaultView;
            s_score.IsReadOnly = true;

            using (DataTable table = set.Tables["s_reb"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["reb"].ColumnName = "场均篮板";
            }
            s_reb.DataContext = set;
            s_reb.ItemsSource = set.Tables["s_reb"].DefaultView;
            s_reb.IsReadOnly = true;

            using (DataTable table = set.Tables["s_assist"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["assist"].ColumnName = "场均助攻";
            }
            s_assist.DataContext = set;
            s_assist.ItemsSource = set.Tables["s_assist"].DefaultView;
            s_assist.IsReadOnly = true;

            using (DataTable table = set.Tables["s_steal"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["steal"].ColumnName = "场均抢断";
            }
            s_steal.DataContext = set;
            s_steal.ItemsSource = set.Tables["s_steal"].DefaultView;
            s_steal.IsReadOnly = true;

            using (DataTable table = set.Tables["s_block"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["blockshot"].ColumnName = "场均盖帽";
            }
            s_block.DataContext = set;
            s_block.ItemsSource = set.Tables["s_block"].DefaultView;
            s_block.IsReadOnly = true;
        }

        private void inittoday()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();

            string sql = String.Format("select teamname,name,score,playerid from personaldata_match where date='{0}' order by score desc limit 0,5"
                 , date);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "t_score");

            sql = String.Format("select teamname,name,reb,playerid from personaldata_match where date='{0}' order by reb desc limit 0,5"
                 , date);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "t_reb");

            sql = String.Format("select teamname,name,assist,playerid from personaldata_match where date='{0}' order by assist desc limit 0,5"
                 , date);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "t_assist");

            sql = String.Format("select teamname,name,steal,playerid from personaldata_match where date='{0}' order by steal desc limit 0,5"
                , date);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "t_steal");

            sql = String.Format("select teamname,name,blockshot,playerid from personaldata_match where date='{0}' order by blockshot desc limit 0,5"
                , date);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "t_block");

            using (DataTable table = set.Tables["t_score"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["score"].ColumnName = "得分";
            }
            t_score.DataContext = set;
            t_score.ItemsSource = set.Tables["t_score"].DefaultView;
            t_score.IsReadOnly = true;

            using (DataTable table = set.Tables["t_reb"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["reb"].ColumnName = "篮板";
            }
            t_reb.DataContext = set;
            t_reb.ItemsSource = set.Tables["t_reb"].DefaultView;
            t_reb.IsReadOnly = true;

            using (DataTable table = set.Tables["t_assist"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["assist"].ColumnName = "助攻";
            }
            t_assist.DataContext = set;
            t_assist.ItemsSource = set.Tables["t_assist"].DefaultView;
            t_assist.IsReadOnly = true;

            using (DataTable table = set.Tables["t_steal"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["steal"].ColumnName = "抢断";
            }
            t_steal.DataContext = set;
            t_steal.ItemsSource = set.Tables["t_steal"].DefaultView;
            t_steal.IsReadOnly = true;

            using (DataTable table = set.Tables["t_block"])
            {
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["name"].ColumnName = "球员";
                table.Columns["blockshot"].ColumnName = "盖帽";
            }
            t_block.DataContext = set;
            t_block.ItemsSource = set.Tables["t_block"].DefaultView;
            t_block.IsReadOnly = true;
        }

        private void initcomobox()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter("select seasonid from season",conn);
            adapter.Fill(set, "season");
            mseason.DataContext = set;
            mseason.ItemsSource = set.Tables["season"].DefaultView;
            mseason.DisplayMemberPath = "seasonid";
            mseason.SelectedValuePath = "seasonid";
            mseason.SelectedIndex = 0;
        }

        private void mseason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            s = (int)mseason.SelectedValue;
            initseason();
        }

        private void t_block_Loaded(object sender, RoutedEventArgs e)
        {
            t_block.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void t_steal_Loaded(object sender, RoutedEventArgs e)
        {
            t_steal.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void t_assist_Loaded(object sender, RoutedEventArgs e)
        {
            t_assist.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void t_reb_Loaded(object sender, RoutedEventArgs e)
        {
            t_reb.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void t_score_Loaded(object sender, RoutedEventArgs e)
        {
            t_score.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void s_score_Loaded(object sender, RoutedEventArgs e)
        {
            if(s_score.Columns.Count()>0)
                s_score.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void s_reb_Loaded(object sender, RoutedEventArgs e)
        {
             if(s_reb.Columns.Count()>0)
                 s_reb.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void s_assist_Loaded(object sender, RoutedEventArgs e)
        {
            if(s_assist.Columns.Count()>0)
                s_assist.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void s_steal_Loaded(object sender, RoutedEventArgs e)
        {
            if(s_steal.Columns.Count()>0)
                s_steal.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void s_block_Loaded(object sender, RoutedEventArgs e)
        {
            if (s_block.Columns.Count() > 0)
                s_block.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int playerid;
            DataGrid a = (DataGrid)sender;
            if(a.SelectedIndex!=-1)
            {
                playerid=Int16.Parse((a.SelectedItem as DataRowView).Row["playerid"].ToString());
                new player(playerid).Show();
                a.SelectedIndex = -1;
            }
        }
    }
}
