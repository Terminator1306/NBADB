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
    /// UniformWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UniformWindow : Window
    {
        private DBHelper dbHelper;
        private DataSet uniformSet;
        public UniformWindow(int teamid)
        {
            InitializeComponent();
            dbHelper = new DBHelper("nbadb");
            MySqlConnection conn = dbHelper.getCon();
            uniformSet = new DataSet();
            String SQLStr;
            SQLStr = "select name from team where teamid = " + teamid;
            MySqlDataAdapter nameAdapter = new MySqlDataAdapter(SQLStr, conn);
            nameAdapter.Fill(uniformSet, "teamName");
            teamLabel.Content = uniformSet.Tables["teamName"].Rows[0]["name"];

            SQLStr = String.Format(
                "select color, sponsor, convert(starttime, char(12)) as starttime, convert(endtime, char(12)) as endtime, picture " +
                "from uniform, uniformbelongs " +
                "where teamid = {0} and " +
                    "uniform.uniformid = uniformbelongs.uniformid"
                    , teamid);
            MySqlDataAdapter infoAdapter = new MySqlDataAdapter(SQLStr, conn);
            infoAdapter.Fill(uniformSet, "uniformInfo");
            using (DataTable table = uniformSet.Tables["uniformInfo"])
            {
                int colorInt;
                table.Columns.Add("主色调");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    colorInt = (int)table.Rows[i]["color"];
                    table.Rows[i]["主色调"] = (colorInt == 1) ? "深色" : "浅色";
                }
                //table.Columns["color"].ColumnName = "主色调";
                table.Columns.Remove("color");
                table.Columns["主色调"].SetOrdinal(0);
                table.Columns["sponsor"].ColumnName = "赞助商";
                table.Columns["starttime"].ColumnName = "使用开始时间";
                table.Columns["endtime"].ColumnName = "使用结束时间";
                uniformGrid.DataContext = uniformSet;
                uniformGrid.ItemsSource = table.DefaultView;
                uniformGrid.IsReadOnly = true;
                if (table.Rows.Count > 0)
                {
                    try
                    {
                        Uri uri = new Uri(table.Rows[0]["picture"].ToString());
                        BitmapImage bitmap = new BitmapImage(uri);
                        uniformImage.Source = bitmap;
                    }
                    catch (Exception)
                    {
                        uniformImage.Source = null;
                    }
                }
            }

        }

        private void uniformGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String src = "";
            DataGrid a = (DataGrid)sender;
            if (a.SelectedIndex != -1)
            {
                src = (a.SelectedItem as DataRowView).Row["picture"].ToString();
                try
                {
                    Uri uri = new Uri(src);
                    BitmapImage bitmap = new BitmapImage(uri);
                    uniformImage.Source = bitmap;
                }
                catch (Exception)
                {
                    uniformImage.Source = null;
                }
                a.SelectedIndex = -1;
            }
        }
        private void uniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (uniformGrid.Columns.Count > 0)
            {
                uniformGrid.Columns[4].Visibility = Visibility.Collapsed;
            }
        }

    }
}
