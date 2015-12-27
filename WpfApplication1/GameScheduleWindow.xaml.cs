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
    /// Interaction logic for GameScheduleWindow.xaml
    /// </summary>
    public partial class GameScheduleWindow : Window
    {
        private string date;
        public GameScheduleWindow()
        {
            InitializeComponent();
            initbox();
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void initbox()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            DataSet set = new DataSet();
            string sql = string.Format("select distinct DATE_FORMAT(date,'%Y-%m-%d') as date from match_schedule order by date desc");
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "date");
            date_box.DataContext = set;
            date_box.ItemsSource = set.Tables["date"].DefaultView;
            date_box.DisplayMemberPath = "date";
            date_box.SelectedValuePath = "date";
            date_box.SelectedIndex = 0;
        }

        private void initrank()
        {
            DBHelper dbhelper = new DBHelper("nbadb");
            MySqlConnection con = dbhelper.getCon();
            DataSet set = new DataSet();
            string sql = String.Format("select gameid,hometeam,CONCAT(convert(homescore,CHAR(3)),':',convert(visitscore,char(3))) AS score , visitteam,progress,judgename,type from match_schedule where date='{0}' ",date);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
            adapter.Fill(set, "rank");
            using (DataTable table = set.Tables["rank"])
            {
                table.Columns.Add("pro", typeof(string));
                table.Columns.Add("t", typeof(string));
                foreach (DataRow row in table.Rows)
                {
                    int type = (int)row["type"];
                    int progress = (int)row["progress"];
                    string t_str;
                    string p_str;
                    switch (progress)
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
                        case 5:
                            p_str = "已结束";
                            break;
                        default:
                            p_str = "未开始";
                            break;
                    }
                    switch(type)
                    {
                        case 1:
                            t_str = "常规赛";
                            break;
                        case 2:
                            t_str = "季后赛";
                            break;
                        case 3:
                            t_str = "总决赛";
                            break;
                        default:
                            t_str = "季前赛";
                            break;
                    }
                    row["t"] = t_str;
                    row["pro"] = p_str;
                }
                table.Columns.Remove("progress");
                table.Columns.Remove("type");
                table.Columns["pro"].SetOrdinal(1);
                table.Columns["hometeam"].ColumnName = "主队";
                table.Columns["score"].ColumnName = "比分";
                table.Columns["visitteam"].ColumnName = "客队";
                table.Columns["pro"].ColumnName = "状态";
                table.Columns["judgename"].ColumnName = "裁判";
                table.Columns["t"].ColumnName = "类型";
            }
            schedule.DataContext = set;
            schedule.ItemsSource = set.Tables["rank"].DefaultView;
            schedule.IsReadOnly = true;
        }

        private void schedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int gameid;
            DataGrid a = (DataGrid)sender;
            if (a.SelectedIndex != -1)
            {
                gameid = Int16.Parse((a.SelectedItem as DataRowView).Row["gameid"].ToString());
                new MatchDetail(gameid).Show();
                a.SelectedIndex = -1;
            }
        }

        private void date_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            date = (string)date_box.SelectedValue;
            initrank();
        }

        private void schedule_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (schedule.Columns.Count() > 0)
                schedule.Columns[0].Visibility = Visibility.Collapsed;
        }
    }
}
