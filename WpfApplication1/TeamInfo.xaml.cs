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
using System.IO;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;

namespace WpfApplication1
{
    /// <summary>
    /// TeamInfoWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class TeamInfoWindow : Window
    {
        //private DataSet dataSet;
        //private MySqlDataAdapter adapter;
        private DBHelper dbHelper;
        public TeamInfoWindow()
        {
            InitializeComponent();

            dbHelper = new DBHelper("nbadb2");
            MySqlConnection conn = dbHelper.getCon();

            DataSet teamSet = new DataSet("teamSet");
            MySqlCommand teamCmd = new MySqlCommand("select teamid, name from team", conn);
            MySqlDataAdapter teamAdapter = new MySqlDataAdapter(teamCmd);
            teamAdapter.Fill(teamSet, "teamName");
            teamBox.DataContext = teamSet;
            teamBox.ItemsSource = teamSet.Tables["teamName"].DefaultView;
            teamBox.DisplayMemberPath = "name";
            teamBox.SelectedValuePath = "teamid";
            teamBox.SelectedIndex = 0;
            
            //DataSet dataSet = new DataSet();
            //String SQLStr = "select team.name as tn, mainCoach.name as cn, coaching.starttime, coaching.endtime " +
            //    "from team, mainCoach, coaching " +
            //    "where team.teamid = coaching.teamid and mainCoach.coachid = coaching.coachid";
            //MySqlCommand cmd = new MySqlCommand(SQLStr, conn);
            //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            //adapter.Fill(dataSet, "coaching");
            //teamGrid.DataContext = dataSet.Tables["coaching"];
            //using (DataTable table = dataSet.Tables["coaching"])
            //{
            //    table.Columns["tn"].ColumnName = "球队";
            //    table.Columns["cn"].ColumnName = "教练";
            //    table.Columns["starttime"].ColumnName = "执教开始时间";
            //    table.Columns["endtime"].ColumnName = "执教结束时间";
            //}
            //teamGrid.ItemsSource = dataSet.Tables["coaching"].DefaultView;
            //teamGrid.IsReadOnly = true;

            conn.Close();
        }

        private void TeamSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //initialize.
            MySqlConnection conn = dbHelper.getCon();
            int teamid = (int)teamBox.SelectedValue;
            String SQLStr = String.Format(
                "select team.name as tn, country, team.city, team.founddate, disbanddate, mainCoach.name as cn, squarename, mainCoach.picture " +
                "from team, usedSquare, coaching, mainCoach " +
                "where " +
                    "team.teamid = {0} and " +
                    "team.teamid = usedSquare.teamid and " +
                    "team.teamid = coaching.teamid and " +
                    "coaching.coachid = mainCoach.coachid ",
                    teamid);
            //MySqlCommand cmd = new MySqlCommand(SQLStr, conn);
            //获取球队基本信息
            MySqlDataAdapter infoAdapter = new MySqlDataAdapter(SQLStr, conn);
            DataSet SQLInfoSet = new DataSet();
            infoAdapter.Fill(SQLInfoSet, "BaseInfo");
            DataSet infoSet = new DataSet();
            using (DataTable table = SQLInfoSet.Tables["BaseInfo"])
            {
                //DateTime date;
                infoSet.Tables.Add("BaseInfo");
                infoSet.Tables["BaseInfo"].Columns.Add("key");
                infoSet.Tables["BaseInfo"].Columns.Add("value");
                DataRow row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "球队名称";
                row["value"] = table.Rows[0]["tn"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
                row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "所属国家";
                row["value"] = table.Rows[0]["country"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
                row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "城市";
                row["value"] = table.Rows[0]["city"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
                row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "建队日期";
                //try
                //{
                //    date = DateTime.Parse(table.Rows[0]["founddate"].ToString());
                //    row["value"] = date.ToString("yyyy-mm-dd");
                //}
                //catch (FormatException)
                //{
                //    row["value"] = table.Rows[0]["founddate"].ToString();
                //}
                row["value"] = table.Rows[0]["founddate"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
                row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "解散日期";
                //try
                //{
                //    date = DateTime.Parse(table.Rows[0]["disbanddate"].ToString());
                //    row["value"] = date.ToString("yyyy-mm-dd");
                //}
                //catch(FormatException){
                //    row["value"] = table.Rows[0]["disbanddate"].ToString();
                //}
                row["value"] = table.Rows[0]["disbanddate"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
                row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "主教练";
                row["value"] = table.Rows[0]["cn"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
                row = infoSet.Tables["BaseInfo"].NewRow();
                row["key"] = "球场";
                row["value"] = table.Rows[0]["squarename"].ToString();
                infoSet.Tables["BaseInfo"].Rows.Add(row);
            }
            baseInfoGrid.DataContext = infoSet;
            baseInfoGrid.ItemsSource = infoSet.Tables[0].DefaultView;
            baseInfoGrid.IsReadOnly = true;
            String s = SQLInfoSet.Tables["BaseInfo"].Rows[0]["picture"].ToString();
            if (!s.Equals("") && File.Exists(s))
            {
                Uri uri = new Uri(s);
                BitmapImage bitmap = new BitmapImage(uri);
                coachImage.Source = bitmap;
            }
            else
            {
                coachImage.Source = null;
            }

            //获取球队球员信息
            SQLStr = String.Format(
                "select player.name, playerbelongs.number, position, " +
                "birthday, height, weight, starttime, endtime " +
                "from player, playerbelongs " +
                "where " +
                    "playerbelongs.teamid = {0} and " +
                    "playerbelongs.playerid = player.playerid",
                    teamid);
            MySqlDataAdapter playerAdapter = new MySqlDataAdapter(SQLStr, conn);
            DataSet playerSet = new DataSet();
            playerAdapter.Fill(playerSet, "playerInfo");
            using (DataTable table = playerSet.Tables["playerInfo"])
            {
                table.Columns.Add("posStr", typeof(string));
                //table.Columns["position"].DataType = typeof(string);
                foreach (DataRow row in table.Rows)
                {
                    //DateTime date;
                    int position = (int)row["position"];
                    String posStr;
                    switch (position)
                    {
                        case 1:
                            posStr = "控球后卫";
                            break;
                        case 2:
                            posStr = "得分后卫";
                            break;
                        case 3:
                            posStr = "小前锋";
                            break;
                        case 4:
                            posStr = "大前锋";
                            break;
                        case 5:
                            posStr = "中锋";
                            break;
                        default:
                            posStr = "其他";
                            break;
                    }
                    row["posStr"] = posStr;

                    
                }
                table.Columns.Remove("position");
                table.Columns["posStr"].SetOrdinal(2);
                table.Columns["name"].ColumnName = "姓名";
                table.Columns["number"].ColumnName = "号码";
                table.Columns["posStr"].ColumnName = "位置";
                table.Columns["birthday"].ColumnName = "生日";
                table.Columns["height"].ColumnName = "身高";
                table.Columns["weight"].ColumnName = "体重";
                table.Columns["starttime"].ColumnName = "加入时间";
                table.Columns["endtime"].ColumnName = "退出时间";
            }
            teamGrid.DataContext = playerSet;
            teamGrid.ItemsSource = playerSet.Tables["playerInfo"].DefaultView;
            teamGrid.IsReadOnly = true;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
