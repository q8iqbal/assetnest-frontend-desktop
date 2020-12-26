using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
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
using assetnest_wpf.View.Dashboard;
using assetnest_wpf.View.Auth;

namespace assetnest_wpf
{
    public partial class MainWindow : MyWindow
    {
       // private MyPage profilePage;
        private Page authPage;
        public MainWindow()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | 
                                                   SecurityProtocolType.Tls11 | 
                                                   SecurityProtocolType.Tls12;
            //profilePage = new ProfilePage();
            authPage = new AuthPage();
            //listUserPage = new ListUserPage(mainFrame);
            mainFrame.Navigate(authPage);
        }
    }
}
