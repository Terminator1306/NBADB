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
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for player.xaml
    /// </summary>
    public partial class player : Window
    {
        private int id;
        public player(int n)
        {
            id = n;
            InitializeComponent();
            initinfo();
            initdata();
        }
        private void initinfo()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            DataSet set = new DataSet();
            string sql = string.Format("select player.name as name,number,birthday,height,weight,armspan,position,jump,picture,careerbegin,team.name As teamname from player,playerbelongs,team where player.playerid={0} and player.playerid=playerbelongs.playerid and endtime is null and playerbelongs.teamid=team.teamid",id);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader mysqlread = cmd.ExecuteReader();
            if(mysqlread.Read())
            {
                string p_str;
                if (!mysqlread.IsDBNull(0))
                {
                    name.Content = mysqlread.GetString(0);
                }
                if (!mysqlread.IsDBNull(1))
                    num.Content = mysqlread.GetString(1);
                if (!mysqlread.IsDBNull(2))
                    birthday.Content = mysqlread.GetString(2).Substring(0,10);
                if (!mysqlread.IsDBNull(3))
                    height.Content = mysqlread.GetString(3)+"cm";
                if (!mysqlread.IsDBNull(5))
                    armlength.Content = mysqlread.GetString(5)+"cm";
                if (!mysqlread.IsDBNull(7))
                    jump.Content = mysqlread.GetString(7) + "cm";
                if (!mysqlread.IsDBNull(4))
                    weight.Content = mysqlread.GetString(4) + "cm";
                if (!mysqlread.IsDBNull(10))
                    team.Content = mysqlread.GetString(10);
                if (!mysqlread.IsDBNull(9))
                    jointime.Content=mysqlread.GetString(9).Substring(0, 10);
                if(!mysqlread.IsDBNull(8))
                {
                    try
                    {
                        string photo_path = mysqlread.GetString(8).ToString();
                        Uri uri = new Uri(photo_path);
                        BitmapImage bitmap = new BitmapImage(uri);
                        photo.Source = bitmap;
                    }catch(Exception)
                    {
                        
                    }
                }
                else
                {
                    Uri uri = new Uri("http://a0.att.hudong.com/21/39/19300542065875137541396894347_950.jpg");
                    BitmapImage bitmap = new BitmapImage(uri);
                    photo.Source = bitmap;
                }
                switch(mysqlread.GetInt32(6))
                {
                    case 1:
                        p_str = "控球后卫";
                        break;
                    case 2:
                        p_str = "得分后卫";
                        break;
                    case 3:
                        p_str = "小前锋";
                        break;
                    case 4:
                        p_str = "大前锋";
                        break;
                    default:
                        p_str = "中锋";
                        break;
                }
                position.Content = p_str;

            }
            conn.Close();
        }

        private void initdata()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            DataSet set = new DataSet();

            string sql = String.Format(
                "select season,teamname,gamenum,round((score/gamenum),2) as score,"+
                "round(totalhit/totalnum,2) as rate,"+
                "round(((frontreb+backreb)/gamenum),2) as reb ,"+
                "round((assist/gamenum),2) as assist,"+
                "round((steal/gamenum),2) as steal,"+
                "round((blockshot/gamenum),2) as blockshot "+
                "from personaldata_season where playerid={0} order by season desc",id);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "data");
            using (DataTable table = set.Tables["data"])
            {
                table.Columns["gamenum"].ColumnName = "出场次数";
                table.Columns["season"].ColumnName = "赛季";
                table.Columns["teamname"].ColumnName = "球队";
                table.Columns["score"].ColumnName = "得分";
                table.Columns["rate"].ColumnName = "命中率";
                table.Columns["reb"].ColumnName = "篮板";
                table.Columns["assist"].ColumnName = "助攻";
                table.Columns["steal"].ColumnName = "抢断";
                table.Columns["blockshot"].ColumnName = "盖帽";
            }
            data.DataContext = set;
            data.ItemsSource = set.Tables["data"].DefaultView;
            data.IsReadOnly = true;
        }
    }
}
