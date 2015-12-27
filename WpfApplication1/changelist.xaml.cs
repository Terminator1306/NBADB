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
    /// Interaction logic for changelist.xaml
    /// </summary>
    public partial class changelist : Window
    {
        private DBHelper dbHelper;
        public changelist()
        {
            InitializeComponent();
            dbHelper = new DBHelper("nbadb");
            init();
        }
      
        private void init()
        {
            MySqlConnection conn = dbHelper.getCon();
            DataSet gameSet = new DataSet();
            String SQLStr = "select gameid, type, progress, hometeam, CONCAT(convert(homescore,CHAR(3)),':',convert(visitscore,char(3))) AS score, visitteam, convert(date, char(12)) as date, judgename " +
                "from match_schedule " + "order by date desc";
            //where progress<5 
            MySqlDataAdapter gameAdapter = new MySqlDataAdapter(SQLStr, conn);
            gameAdapter.Fill(gameSet, "gameList");
            using (DataTable table = gameSet.Tables["gameList"])
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
                    switch (type)
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
                table.Columns["t"].SetOrdinal(1);
                table.Columns["t"].ColumnName = "类型";
                table.Columns["pro"].SetOrdinal(1);
                table.Columns["pro"].ColumnName = "比赛进度";
                table.Columns["hometeam"].ColumnName = "主队";
                table.Columns["visitteam"].ColumnName = "客队";
                table.Columns["score"].ColumnName = "比分";
                table.Columns["date"].ColumnName = "日期";
                table.Columns["judgename"].ColumnName = "裁判";
                table.Columns.Remove("type");
                table.Columns.Remove("progress");
                gameListGrid.DataContext = gameSet;
                gameListGrid.ItemsSource = gameSet.Tables["gameList"].DefaultView;
                gameListGrid.IsReadOnly = true;
            }
            conn.Close();
            gameListGrid.SelectedIndex = -1;
        }
        private void gameListGrid_Loaded(object sender, RoutedEventArgs e){
            if (gameListGrid.Columns.Count > 0)
            {
                gameListGrid.Columns[0].Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameListGrid.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要修改的比赛", "提示", MessageBoxButton.OK);
                return;
            }
            int gameid = (int)(gameListGrid.SelectedItem as DataRowView)["gameid"];
            new change(gameid).Show();
            Close();
            init();
            if (gameListGrid.Columns.Count > 0)
            {
                gameListGrid.Columns[0].Visibility = Visibility.Collapsed;
            }
        }
    }
}
