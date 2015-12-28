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
    /// Interaction logic for MatchDetail.xaml
    /// </summary>
    public partial class MatchDetail : Window
    {
        private int gameid;
        private int homeid;
        private int visitid;
        public MatchDetail(int id)
        {
            gameid = id;
            InitializeComponent();
            initinfo();
            initdata();
        }

        private void initinfo()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();
            string sql = string.Format("select progress,homeid,visitid,hometeam,visitteam,homescore,visitscore,homephoto,visitphoto,date from match_schedule where gameid={0}",gameid);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader mysqlread = cmd.ExecuteReader();
            if(mysqlread.Read())
            {
                string p_str;
                if (!mysqlread.IsDBNull(0))
                {
                    switch (mysqlread.GetInt32(0))
                    {
                        case 0:
                            p_str = "未开始";
                            break;
                        case 1:
                            p_str = "第一节";
                            break;
                        case 2:
                            p_str = "第二节";
                            break;
                        case 3:
                            p_str = "第三节";
                            break;
                        case 4:
                            p_str = "第四节";
                            break;
                        default:
                            p_str = "已结束";
                            break;
                    }
                    condition.Content = p_str;
                }
                if (!mysqlread.IsDBNull(1))
                {
                    homeid = mysqlread.GetInt32(1);
                }
                if (!mysqlread.IsDBNull(2))
                {
                    visitid = mysqlread.GetInt32(2);
                }
                if (!mysqlread.IsDBNull(3))
                {
                    hometeam1.Content = hometeam.Content = mysqlread.GetString(3);
                }
                if (!mysqlread.IsDBNull(4))
                {
                    visitteam1.Content = visitteam.Content = mysqlread.GetString(4);
                }
                if (!mysqlread.IsDBNull(5))
                    homescore.Content = mysqlread.GetString(5);
                if (!mysqlread.IsDBNull(6))
                    visitscore.Content = mysqlread.GetString(6);
                if (!mysqlread.IsDBNull(9))
                {
                    date.Content = mysqlread.GetString(9).Substring(0,10);
                }
                if (!mysqlread.IsDBNull(7))
                {
                    try
                    {
                        string photo_path = mysqlread.GetString(7).ToString();
                        Uri uri = new Uri(photo_path);
                        BitmapImage bitmap = new BitmapImage(uri);
                        home_p.Source = bitmap;
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    Uri uri = new Uri("http://a0.att.hudong.com/21/39/19300542065875137541396894347_950.jpg");
                    BitmapImage bitmap = new BitmapImage(uri);
                    home_p.Source = bitmap;
                }

                if (!mysqlread.IsDBNull(8))
                {
                    try
                    {
                        string photo_path = mysqlread.GetString(8).ToString();
                        Uri uri = new Uri(photo_path);
                        BitmapImage bitmap = new BitmapImage(uri);
                        visit_p.Source = bitmap;
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    Uri uri = new Uri("http://a0.att.hudong.com/21/39/19300542065875137541396894347_950.jpg");
                    BitmapImage bitmap = new BitmapImage(uri);
                    visit_p.Source = bitmap;
                }
            }
            conn.Close();
        }
        private void initdata()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();
            string sql = string.Format("select playerid,name,playtime,CONCAT(convert(totalhit,CHAR(3)),'-',convert(totalnum,char(3))) AS shot,"+
                "CONCAT(convert(3phits,CHAR(3)),'-',convert(3pnum,char(3))) AS 3shout,"+
                "CONCAT(convert(penaltyhits,CHAR(3)),'-',convert(penaltynum,char(3))) AS penalty,"+
                "assist,frontreb,backreb,frontreb+backreb as reb,blockshot,steal,foul,score from personaldata_match where teamid={0} and gameid={1} order by first desc",homeid,gameid);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "data_h");
            using (DataTable table = set.Tables["data_h"])
            {
                table.Columns["name"].ColumnName = "姓名";
                table.Columns["playtime"].ColumnName = "时间";
                table.Columns["shot"].ColumnName = "投篮";
                table.Columns["3shout"].ColumnName = "3分";
                table.Columns["penalty"].ColumnName = "罚球";
                table.Columns["frontreb"].ColumnName = "前篮板";
                table.Columns["backreb"].ColumnName = "后篮板";
                table.Columns["reb"].ColumnName = "篮板";
                table.Columns["assist"].ColumnName = "助攻";
                table.Columns["steal"].ColumnName = "抢断";
                table.Columns["blockshot"].ColumnName = "盖帽";
                table.Columns["foul"].ColumnName = "犯规";
                table.Columns["score"].ColumnName = "得分";
            }
            home_d.DataContext = set;
            home_d.ItemsSource = set.Tables["data_h"].DefaultView;
            home_d.IsReadOnly = true;

            sql = string.Format("select playerid,name,playtime,CONCAT(convert(totalhit,CHAR(3)),'-',convert(totalnum,char(3))) AS shot," +
                "CONCAT(convert(3phits,CHAR(3)),'-',convert(3pnum,char(3))) AS 3shout," +
                "CONCAT(convert(penaltyhits,CHAR(3)),'-',convert(penaltynum,char(3))) AS penalty," +
                "assist,frontreb,backreb,frontreb+backreb as reb,blockshot,steal,foul,score from personaldata_match where teamid={0} and gameid={1} order by first desc", visitid, gameid);
            adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "data");
            using (DataTable table = set.Tables["data"])
            {
                table.Columns["name"].ColumnName = "姓名";
                table.Columns["playtime"].ColumnName = "时间";
                table.Columns["shot"].ColumnName = "投篮";
                table.Columns["3shout"].ColumnName = "3分";
                table.Columns["penalty"].ColumnName = "罚球";
                table.Columns["frontreb"].ColumnName = "前篮板";
                table.Columns["backreb"].ColumnName = "后篮板";
                table.Columns["reb"].ColumnName = "篮板";
                table.Columns["assist"].ColumnName = "助攻";
                table.Columns["steal"].ColumnName = "抢断";
                table.Columns["blockshot"].ColumnName = "盖帽";
                table.Columns["foul"].ColumnName = "犯规";
                table.Columns["score"].ColumnName = "得分";
            }
            visit_d.DataContext = set;
            visit_d.ItemsSource = set.Tables["data"].DefaultView;
            visit_d.IsReadOnly = true;
        }

        private void d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int playerid;
            DataGrid a = (DataGrid)sender;
            if (a.SelectedIndex != -1)
            {
                playerid = Int16.Parse((a.SelectedItem as DataRowView).Row["playerid"].ToString());
                new player(playerid).Show();
                a.SelectedIndex = -1;
            }
        }

        private void home_d_Loaded(object sender, RoutedEventArgs e)
        {
            if (home_d.Columns.Count() > 0)
                home_d.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void visit_d_Loaded(object sender, RoutedEventArgs e)
        {
            if (visit_d.Columns.Count() > 0)
                visit_d.Columns[0].Visibility = Visibility.Collapsed;
        }
    }
}
