using assetnest_wpf.View.Dashboard;
using assetnest_wpf.View.ListUser;
using assetnest_wpf.View.Profile;
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

namespace assetnest_wpf
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Application.Current.MainWindow.Height = 550;
            secondFrame.Navigate(new DashboardPage());
        }

        private void menuHome_Click(object sender, RoutedEventArgs e)
        {
            secondFrame.Navigate(new DashboardPage());
        }

        private void menuProfile_Click(object sender, RoutedEventArgs e)
        {
            secondFrame.Navigate(new ProfilePage());
        }

        private void menuStaff_Click(object sender, RoutedEventArgs e)
        {
            secondFrame.Navigate(new ListUserPage(""));
        }
    }
}
