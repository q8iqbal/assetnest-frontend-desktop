using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Velacro.DataStructures;
using Velacro.UIElements.Basic;
using assetnest_wpf.Profile;
using assetnest_wpf.Employee;

namespace assetnest_wpf
{
    public partial class MainWindow : MyWindow
    {
        private MyPage profilePage;
        private MyPage addUser;
        public MainWindow()
        {
            InitializeComponent();
            profilePage = new ProfilePage();
            addUser = new AddUser();
        }

        private void profileButton_btn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(profilePage);
        }

        private void saveButton_btn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(addUser);
        }
    }
}
