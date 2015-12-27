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
    /// Interaction logic for AddMatch.xaml
    /// </summary>
    public partial class AddMatch : Window
    {
        private int homeid;
        private int visitid;
        private int judgeid;
        private int gameid;
        private int processid;

        public AddMatch()
        {
            InitializeComponent();
            initprocess_s();
            inittype_s();
            inithome_s();
            initvisit_s();
            initjudge_s();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hometeam_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hometeam_box.SelectedValue != null)
            {
                homeid = (int)hometeam_box.SelectedValue;
                DBHelper dbHelper = new DBHelper("nbadb");
                MySqlConnection conn = dbHelper.getCon();
                string sql = string.Format("select photo,name from team where teamid={0}", homeid);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader mysqlread = cmd.ExecuteReader();
                if (mysqlread.Read())
                {
                    if (!mysqlread.IsDBNull(0))
                    {
                        try
                        {
                            string photo_path = mysqlread.GetString(0).ToString();
                            Uri uri = new Uri(photo_path);
                            BitmapImage bitmap = new BitmapImage(uri);
                            homephoto.Source = bitmap;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        Uri uri = new Uri("http://a0.att.hudong.com/21/39/19300542065875137541396894347_950.jpg");
                        BitmapImage bitmap = new BitmapImage(uri);
                        homephoto.Source = bitmap;
                    }
                    if (!mysqlread.IsDBNull(1))
                        hometeam.Content = mysqlread.GetString(1).ToString();
                }
                inithome_d();
            }
        }

        private void visitteam_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (visitteam_box.SelectedValue != null)
            {
                visitid = (int)visitteam_box.SelectedValue;
                DBHelper dbHelper = new DBHelper("nbadb");
                MySqlConnection conn = dbHelper.getCon();
                string sql = string.Format("select photo,name from team where teamid={0}", visitid);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader mysqlread = cmd.ExecuteReader();
                if (mysqlread.Read())
                {
                    if (!mysqlread.IsDBNull(0))
                    {
                        try
                        {
                            string photo_path = mysqlread.GetString(0).ToString();
                            Uri uri = new Uri(photo_path);
                            BitmapImage bitmap = new BitmapImage(uri);
                            visitphoto.Source = bitmap;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else 
                    {
                        Uri uri = new Uri("http://a0.att.hudong.com/21/39/19300542065875137541396894347_950.jpg");
                        BitmapImage bitmap = new BitmapImage(uri);
                        visitphoto.Source = bitmap;
                    }
                    if(!mysqlread.IsDBNull(1))
                        visitteam.Content = mysqlread.GetString(1).ToString();
                }
                initvisit_d();
            }
        }

        private void judge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (judge.SelectedValue != null)
                judgeid =(int)judge.SelectedValue;
        }

        private void process_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            processid = (int)process.SelectedValue;
        }
        
        private void inittype_s()
        {
            DataTable table = new DataTable();
            table.Columns.Add("text", Type.GetType("System.String"));
            table.Columns.Add("value", Type.GetType("System.Int32"));
            DataRow newRow = table.NewRow();
            newRow = table.NewRow();
            newRow["text"] = "常规赛";
            newRow["value"] = 1;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "季后赛";
            newRow["value"] = 2;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "总决赛";
            newRow["value"] = 3;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "季前赛";
            newRow["value"] = 4;
            table.Rows.Add(newRow);

            type.DataContext = table;
            type.ItemsSource = table.DefaultView;
            type.DisplayMemberPath = "text";
            type.SelectedValuePath = "value";
            type.SelectedIndex = 0;
        }
        
        private void initprocess_s()
        {
            DataTable table = new DataTable();
            table.Columns.Add("text", Type.GetType("System.String"));
            table.Columns.Add("value", Type.GetType("System.Int32"));
            DataRow newRow = table.NewRow();
            newRow["text"] = "未开始";
            newRow["value"] = 0;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "第一节";
            newRow["value"] = 1;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "第二节";
            newRow["value"] = 2;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "第三节";
            newRow["value"] = 3;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "第四节";
            newRow["value"] = 4;
            table.Rows.Add(newRow);

            newRow = table.NewRow();
            newRow["text"] = "已结束";
            newRow["value"] = 5;
            table.Rows.Add(newRow);

            process.DataContext = table;
            process.ItemsSource = table.DefaultView;
            process.DisplayMemberPath = "text";
            process.SelectedValuePath = "value";
            process.SelectedIndex = 0;
        }
        private void inithome_s()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();
            string sql = "select teamid,name from team";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "team");
            hometeam_box.DataContext = set;
            hometeam_box.ItemsSource = set.Tables["team"].DefaultView;
            hometeam_box.DisplayMemberPath = "name";
            hometeam_box.SelectedValuePath = "teamid";
            hometeam_box.SelectedIndex = 0;
        }
        private void initvisit_s()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            DataSet set = new DataSet();
            string sql = "select teamid,name from team";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "team");
            visitteam_box.DataContext = set;
            visitteam_box.ItemsSource = set.Tables["team"].DefaultView;
            visitteam_box.DisplayMemberPath = "name";
            visitteam_box.SelectedValuePath = "teamid";
            visitteam_box.SelectedIndex = 1;
        }
        private void initjudge_s()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            DataSet set = new DataSet();
            string sql = "select judgeid,name from mainjudge";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "judge");
            judge.DataContext = set;
            judge.ItemsSource = set.Tables["judge"].DefaultView;
            judge.DisplayMemberPath = "name";
            judge.SelectedValuePath = "judgeid";
            judge.SelectedIndex = 0;
        }
        public class ListItem
        {

            private string textField;
            public string TextField
            {
                get { return textField; }
                set { textField = value; }
            }
            private int valueField;
            public int ValueField
            {
                get { return valueField; }
                set { valueField = value; }
            }
            public ListItem(string text,int value)
            {
                textField = text;
                valueField = value;
            }
        }
        private void inithome_d()
        {
            DataTable table =new DataTable();
            table.Columns.Add("id", Type.GetType("System.Int32"));
            table.Columns.Add("姓名", Type.GetType("System.String"));
            table.Columns.Add("首发", Type.GetType("System.Int32"));
            table.Columns.Add("时间", Type.GetType("System.Int32"));
            table.Columns.Add("2分出手", Type.GetType("System.Int32"));
            table.Columns.Add("2分命中", Type.GetType("System.Int32"));
            table.Columns.Add("3分出手", Type.GetType("System.Int32"));
            table.Columns.Add("3分命中", Type.GetType("System.Int32"));
            table.Columns.Add("罚球次数", Type.GetType("System.Int32"));
            table.Columns.Add("罚球命中", Type.GetType("System.Int32"));
            table.Columns.Add("失误", Type.GetType("System.Int32"));
            table.Columns.Add("助攻", Type.GetType("System.Int32"));
            table.Columns.Add("前场篮板", Type.GetType("System.Int32"));
            table.Columns.Add("后场篮板", Type.GetType("System.Int32"));
            table.Columns.Add("盖帽", Type.GetType("System.Int32"));
            table.Columns.Add("抢断", Type.GetType("System.Int32"));
            table.Columns.Add("犯规", Type.GetType("System.Int32"));
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            string sql = string.Format("select player.playerid,name from player,playerbelongs where player.playerid=playerbelongs.playerid and teamid={0}",homeid);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader mysqlread = cmd.ExecuteReader();
            while(mysqlread.Read())
            {
                DataRow row;
                row = table.NewRow();
                row["姓名"] = mysqlread.GetString(1);
                table.Rows.Add(row);
            }
            conn.Close();
            home_d.ItemsSource = table.DefaultView;

        }
        private void initvisit_d()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id", Type.GetType("System.Int32"));
            table.Columns.Add("姓名", Type.GetType("System.String"));
            table.Columns.Add("首发", Type.GetType("System.Int32"));
            table.Columns.Add("时间", Type.GetType("System.Int32"));
            table.Columns.Add("2分出手", Type.GetType("System.Int32"));
            table.Columns.Add("2分命中", Type.GetType("System.Int32"));
            table.Columns.Add("3分出手", Type.GetType("System.Int32"));
            table.Columns.Add("3分命中", Type.GetType("System.Int32"));
            table.Columns.Add("罚球次数", Type.GetType("System.Int32"));
            table.Columns.Add("罚球命中", Type.GetType("System.Int32"));
            table.Columns.Add("失误", Type.GetType("System.Int32"));
            table.Columns.Add("助攻", Type.GetType("System.Int32"));
            table.Columns.Add("前场篮板", Type.GetType("System.Int32"));
            table.Columns.Add("后场篮板", Type.GetType("System.Int32"));
            table.Columns.Add("盖帽", Type.GetType("System.Int32"));
            table.Columns.Add("抢断", Type.GetType("System.Int32"));
            table.Columns.Add("犯规", Type.GetType("System.Int32"));
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            string sql = string.Format("select player.playerid,name from player,playerbelongs where player.playerid=playerbelongs.playerid and teamid={0}", visitid);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader mysqlread = cmd.ExecuteReader();
            while (mysqlread.Read())
            {
                DataRow row;
                row = table.NewRow();
                row["姓名"] = mysqlread.GetString(1);
                table.Rows.Add(row);
            }
            conn.Close();
            visit_d.ItemsSource = table.DefaultView;
        }

        private void home_d_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (home_d.Columns.Count() > 0)
            {
                home_d.Columns[0].Visibility = Visibility.Collapsed;
                home_d.Columns[1].IsReadOnly = true;
            }
        }

        private void visit_d_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (visit_d.Columns.Count() > 0)
            {
                visit_d.Columns[0].Visibility = Visibility.Collapsed;
                home_d.Columns[1].IsReadOnly = true;
            }
        }
    
        private int create_game()
        {
            int typeid =(int) type.SelectedValue;
            
            DateTime dt=(DateTime)date.SelectedDate;
            string sql = string.Format("insert into game(type,date,progress) values({0},{1},{2})", typeid, dt.ToString("yyyy-MM-dd"), processid);
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            return 1;
        }
    }
}
