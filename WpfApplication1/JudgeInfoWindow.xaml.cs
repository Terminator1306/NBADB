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
using System.IO;


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
            dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            
            DataSet judgeSet = new DataSet();
            MySqlDataAdapter judgeAdapter =
                new MySqlDataAdapter("select judgeid, name from mainJudge", conn);
            judgeAdapter.Fill(judgeSet, "judgeName");
            judgeBox.DataContext = judgeSet;
            judgeBox.ItemsSource = judgeSet.Tables["judgeName"].DefaultView;
            judgeBox.DisplayMemberPath = "name";
            judgeBox.SelectedValuePath = "judgeid";
            judgeBox.SelectedIndex = 0;

            conn.Close();
        }

        private void judgeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySqlConnection conn = dbHelper.getCon();
            int judgeid = (int)judgeBox.SelectedValue;
            DataSet judgeSet = new DataSet();
            String SQLStr, dateSQL;
            dateSQL = "convert(birthday, char(12)) as birthday";
            SQLStr = String.Format(
                "select mainJudge.name as jn, gender, {1}, picture " +
                "from mainJudge " +
                "where " +
                    "mainJudge.judgeid = {0}",
                    judgeid, dateSQL);
            MySqlDataAdapter judgeAdapter = new MySqlDataAdapter(SQLStr, conn);
            judgeAdapter.Fill(judgeSet, "baseInfo");
            dateSQL = "convert(date, char(12)) as date";
            SQLStr = String.Format(
                "select game.type, ht.name as htn, vt.name as vtn, {1}, progress " +
                "from judgeschedule, game, gameschedule, team as ht, team as vt " +
                "where " +
                    "judgeschedule.judgeid = {0} and " +
                    "judgeschedule.gameid = gameschedule.gameid and " +
                    "judgeschedule.gameid = game.gameid and " +
                    "gameschedule.hometeamid = ht.teamid and " +
                    "gameschedule.visitingteamid = vt.teamid ",
                    judgeid, dateSQL);
            judgeAdapter = new MySqlDataAdapter(SQLStr, conn);
            judgeAdapter.Fill(judgeSet, "gameInfo");

            nameLabel.Content = judgeSet.Tables["baseInfo"].Rows[0]["jn"];
            int gender = (int)judgeSet.Tables["baseInfo"].Rows[0]["gender"];
            genderLabel.Content = (gender == 1) ? "男" : "女";
            birthdayLabel.Content = judgeSet.Tables["baseInfo"].Rows[0]["birthday"];
            String picStr = judgeSet.Tables["baseInfo"].Rows[0]["picture"].ToString();
            try{
                Uri uri = new Uri(picStr);
                BitmapImage bitmap = new BitmapImage(uri);
                judgeImage.Source = bitmap;
            }
            catch(Exception)
            {
                judgeImage.Source = null;
            }

            using (DataTable table = judgeSet.Tables["gameInfo"])
            {
                table.Columns.Add("比赛类型");
                table.Columns.Add("比赛状态");
                int typeInt;
                int progressInt;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    typeInt = (int)table.Rows[i]["type"];
                    progressInt = (int)table.Rows[i]["progress"];
                    string p_str;
                    switch (progressInt)
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
                    table.Rows[i]["比赛状态"] = p_str;
                    switch (typeInt)
                    {
                        case 1:
                            table.Rows[i]["比赛类型"] = "常规赛";
                            break;
                        case 2:
                            table.Rows[i]["比赛类型"] = "季后赛";
                            break;
                        case 3:
                            table.Rows[i]["比赛类型"] = "总决赛";
                            break;
                        case 4:
                            table.Rows[i]["比赛类型"] = "季前赛";
                            break;
                    }
                }
                table.Columns.Remove("type");
                table.Columns["比赛类型"].SetOrdinal(0);
                table.Columns["htn"].ColumnName = "主队名称";
                table.Columns["vtn"].ColumnName = "客队名称";
                //table.Columns.Remove("date");
                //table.Columns["日期"].SetOrdinal(3);
                table.Columns["date"].ColumnName = "日期";
                table.Columns.Remove("progress");
                table.Columns["比赛状态"].SetOrdinal(4);
            }
            judgeInfoGrid.DataContext = judgeSet;
            judgeInfoGrid.ItemsSource = judgeSet.Tables["gameInfo"].DefaultView;
            judgeInfoGrid.IsReadOnly = true;
        }
    }
}
