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
    /// coachInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class coachInfoWindow : Window
    {
        DBHelper dbHelper;
        public coachInfoWindow()
        {
            InitializeComponent();
            dbHelper = new DBHelper("nbadb2");
            MySqlConnection conn = dbHelper.getCon();

            DataSet coachSet = new DataSet();
            MySqlDataAdapter coachAdapter = 
                new MySqlDataAdapter("select coachid, name from mainCoach", conn);
            coachAdapter.Fill(coachSet, "coachName");
            coachBox.DataContext = coachSet;
            coachBox.ItemsSource = coachSet.Tables["coachName"].DefaultView;
            coachBox.DisplayMemberPath = "name";
            coachBox.SelectedValuePath = "coachid";
            coachBox.SelectedIndex = 0;

            conn.Close();
        }

        private void coachBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySqlConnection conn = dbHelper.getCon();
            int coachid = (int)coachBox.SelectedValue;
            String SQLStr, dateSQL, dateSQL2;
            DataSet coachSet = new DataSet();
            dateSQL = "convert((select distinct birthday from mainCoach as t where t.birthday = mainCoach.birthday), char(12)) as birthday";
            SQLStr = String.Format(
                "select mainCoach.name as cn, gender, {1}, picture " +
                "from mainCoach " +
                "where " +
                    "mainCoach.coachid = {0}",
                    coachid, dateSQL);
            MySqlDataAdapter coachAdapter = new MySqlDataAdapter(SQLStr, conn);
            coachAdapter.Fill(coachSet, "SQLBaseInfo");
            dateSQL = "convert((select distinct starttime from coaching as t where t.starttime = coaching.starttime), char(12)) as starttime";
            dateSQL2 = "convert((select distinct endtime from coaching as t where t.endtime = coaching.endtime), char(12)) as endtime";
            SQLStr = String.Format(
                "select team.name as tn, {1}, {2} " +
                "from coaching, team " +
                "where " + 
                    "coaching.coachid = {0} and " +
                    "coaching.teamid = team.teamid ", 
                    coachid, dateSQL, dateSQL2);
            coachAdapter = new MySqlDataAdapter(SQLStr, conn);
            coachAdapter.Fill(coachSet, "SQLCoachingInfo");
            coachSet.Tables.Add("CoachInfo");
            coachSet.Tables["CoachInfo"].Columns.Add("key");
            coachSet.Tables["CoachInfo"].Columns.Add("value");
            using (DataTable table = coachSet.Tables["CoachInfo"])
            {
                DataRow row = table.NewRow();
                row["key"] = "姓名";
                row["value"] = coachSet.Tables["SQLBaseInfo"].Rows[0]["cn"].ToString();
                table.Rows.Add(row);
                row = table.NewRow();
                row["key"] = "性别";
                int gender = (int)coachSet.Tables["SQLBaseInfo"].Rows[0]["gender"];
                row["value"] = gender == 1?"♂":"♀";
                table.Rows.Add(row);
                row = table.NewRow();
                row["key"] = "生日";
                row["value"] = coachSet.Tables["SQLBaseInfo"].Rows[0]["birthday"];
                table.Rows.Add(row);

                String picStr = coachSet.Tables["SQLBaseInfo"].Rows[0]["picture"].ToString();
                if (!picStr.Equals("") && File.Exists(picStr))
                {
                    Uri uri = new Uri(picStr);
                    BitmapImage bitmap = new BitmapImage(uri);
                    coachImage.Source = bitmap;
                }
                else
                {
                    coachImage.Source = null;
                }
            }
            if (coachSet.Tables["SQLCoachingInfo"].Rows.Count > 0)
            {
                using (DataTable table = coachSet.Tables["CoachInfo"])
                {
                    foreach (DataRow row in coachSet.Tables["SQLCoachingInfo"].Rows)
                    {
                        //row["endtime"].ToString();
                        if (row["endtime"].ToString().Equals(""))
                        {
                            DataRow r = table.NewRow();
                            r["key"] = "当前执教球队";
                            r["value"] = row["tn"].ToString();
                            table.Rows.Add(r);
                            r = table.NewRow();
                            r["key"] = "执教开始时间";
                            r["value"] = row["starttime"].ToString();
                            table.Rows.Add(r);
                        }
                    }
                }
            }

            using (DataTable table = coachSet.Tables["SQLCoachingInfo"])
            {
                table.Columns["tn"].ColumnName = "球队名称";
                table.Columns["starttime"].ColumnName = "执教开始时间";
                table.Columns["endtime"].ColumnName = "执教结束时间";
            }

            baseInfoGrid.DataContext = coachSet;
            baseInfoGrid.ItemsSource = coachSet.Tables["CoachInfo"].DefaultView;
            baseInfoGrid.IsReadOnly = true;
            coachingGrid.DataContext = coachSet;
            coachingGrid.ItemsSource = coachSet.Tables["SQLCoachingInfo"].DefaultView;
            coachingGrid.IsReadOnly = true;

            conn.Close();
        }
    }
}
