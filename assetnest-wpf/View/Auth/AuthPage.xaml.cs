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

using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using assetnest_wpf.Utils;
using assetnest_wpf.Profile;

namespace assetnest_wpf.View.Auth
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private AuthController controller;
        private Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(2),
                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

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

        public void onSuccessLogin()
        {
            //ProfilePage page = new ProfilePage();
            //NavigationService.Navigate(page);
            this.onFailedLogin(StorageUtil.Instance.company.name);
        }

        public void onFailedLogin(string message)
        {
            notifier.ShowError(message);
        }
    }
}
