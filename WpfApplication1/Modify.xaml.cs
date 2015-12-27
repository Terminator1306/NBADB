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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Modify.xaml
    /// </summary>
    public partial class Modify : Window
    {
        public Modify()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            new AddMatch().Show();
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            new changelist().Show();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            new DeleteGame().Show();
        }
    }
}
