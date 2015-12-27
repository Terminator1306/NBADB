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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void TodayTopPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new TopPlayer().Show();
        }

        private void GameSchedule_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameScheduleWindow  gs= new GameScheduleWindow();
            gs.Show();
        }

        private void TeamRank_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new TeamRank().Show();
        }

        private void Player_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new playerselect().Show();
        }

        private void Team_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TeamInfoWindow teamWindow = new TeamInfoWindow();
            teamWindow.Show();
        }

        private void Referee_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            JudgeInfoWindow judgeWindows = new JudgeInfoWindow();
            judgeWindows.Show();
        }

        private void Coach_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            coachInfoWindow coachWindow = new coachInfoWindow();
            coachWindow.Show();
        }

        private void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new AddMatch().Show();
        }
    }
}
