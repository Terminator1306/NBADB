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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for playerselect.xaml
    /// </summary>
    public partial class playerselect : Window
    {
        private int teamid;
        private int playerid;
        public playerselect()
        {
            InitializeComponent();
            initteam_s();
        }

        private void player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (player.SelectedValue!=null)
                playerid = (int)player.SelectedValue;
        }

        private void team_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            teamid = (int)team.SelectedValue;
            initplayer_s();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new player(playerid).Show();
        }

        private void initteam_s()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            DataSet set = new DataSet();
            string sql="select teamid,name from team";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql,conn);
            adapter.Fill(set, "team");
            team.DataContext = set;
            team.ItemsSource = set.Tables["team"].DefaultView;
            team.DisplayMemberPath = "name";
            team.SelectedValuePath = "teamid";
            team.SelectedIndex = 0;
        }

        private void initplayer_s()
        {
            DBHelper dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();

            DataSet set = new DataSet();
            string sql = string.Format("select player.playerid,name from player,playerbelongs where teamid={0} and endtime is null and player.playerid=playerbelongs.playerid", teamid);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(set, "player");
            player.DataContext = set;
            player.ItemsSource = set.Tables["player"].DefaultView;
            player.DisplayMemberPath = "name";
            player.SelectedValuePath = "playerid";
            player.SelectedIndex = 0;
        }
    }
}
