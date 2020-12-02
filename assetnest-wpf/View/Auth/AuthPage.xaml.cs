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
using Velacro.UIElements.Basic;

namespace assetnest_wpf.View.Auth
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private AuthController controller;
        public AuthPage()
        {
            InitializeComponent();
            controller = new AuthController(this);
            Application.Current.MainWindow.Height = 500;
            Application.Current.MainWindow.Width = 800;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            g_login.Visibility = Visibility.Hidden;
            g_register_user.Visibility = Visibility.Visible;
        }

        private void bt_next_Click(object sender, RoutedEventArgs e)
        {
            g_register_user.Visibility = Visibility.Hidden;
            g_register_company.Visibility = Visibility.Visible;
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            g_register_user.Visibility = Visibility.Hidden;
            g_login.Visibility = Visibility.Visible;
        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            g_register_user.Visibility = Visibility.Visible;
            g_register_company.Visibility = Visibility.Hidden;
        }

        private void bt_sign_in_Click(object sender, RoutedEventArgs e)
        {
            //ProfilePage page = new ProfilePage();
            //NavigationService.Navigate(page);
            //getController().callMethod("sendLoginRequest", tb_email.Text, tb_password.Password.ToString());
            controller.sendLoginRequest(tb_email.Text, tb_password.Password.ToString());
        }

        public void startLoading()
        {
            pb_loading.Visibility = Visibility.Visible;
        }

        public void endLoading()
        {
            pb_loading.Visibility = Visibility.Hidden;
        }
    }
}
