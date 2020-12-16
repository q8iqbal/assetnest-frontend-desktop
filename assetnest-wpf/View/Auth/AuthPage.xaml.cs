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
using assetnest_wpf.Utils.Validations;
using f = System.Windows.Forms;
using RestSharp;
using assetnest_wpf.Model;
using assetnest_wpf.EditProfile;
using assetnest_wpf.Staff;

namespace assetnest_wpf.View.Auth
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private AuthController controller;
        public string email{ get; set; }
        public string password { get; set; }
        private string uImage = "";
        private string cImage = "";

        public AuthPage()
        {
            InitializeComponent();
            controller = new AuthController(this);
            Application.Current.MainWindow.Height = 500;
            Application.Current.MainWindow.Width = 800;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clearInvalid(g_login);
            g_login.Visibility = Visibility.Hidden;
            g_register_user.Visibility = Visibility.Visible;
        }

        private void bt_next_Click(object sender, RoutedEventArgs e)
        {
            if (!registerUserValidate())
            {
                this.AddValidationAbility(tb_register_password);
                this.AddValidationAbility(tb_register_rpassword);
                //cek if password + confirm password is the same
                if (tb_register_password.Password != tb_register_rpassword.Password)
                {
                    
                    this.UpdateValidation(tb_register_password, "Password not same");
                    this.UpdateValidation(tb_register_rpassword, "Password not same");
                }
                else
                {


                    this.UpdateValidation(tb_register_password, null);
                    this.UpdateValidation(tb_register_rpassword, null);
                    g_register_user.Visibility = Visibility.Hidden;
                    g_register_company.Visibility = Visibility.Visible;
                }
                
            }
            
        }
        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            clearInvalid(g_register_user);
            g_register_user.Visibility = Visibility.Hidden;
            g_login.Visibility = Visibility.Visible;
        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            clearInvalid(g_register_company);
            g_register_user.Visibility = Visibility.Visible;
            g_register_company.Visibility = Visibility.Hidden;
        }

        private void bt_sign_in_Click(object sender, RoutedEventArgs e)
        {
            loginValidate();
            if (!Validation.GetHasError(tb_email) && !Validation.GetHasError(tb_password))
            {
                controller.sendLoginRequest(tb_email.Text, tb_password.Password.ToString());
            }

        }

        private void bt_register_Click(object sender, RoutedEventArgs e)
        {
            if (!registerCompanyValidate())
            {
                User user = new User()
                {
                    name = tb_register_name.Text,
                    email = tb_register_email.Text,
                    password = tb_register_password.Password
                };

                Company company = new Company()
                {
                    name = tb_register_companyName.Text,
                    address = tb_register_address.Text,
                    phone = tb_register_phone.Text,
                    description = tb_register_desc.Text
                };
                controller.sendRegisterRequest(user, company, uImage, cImage);
            }
        }

        private void AddValidationAbility(FrameworkElement uiElement)
        {
            var binding = new Binding("TagProperty");
            binding.Source = this;

            uiElement.SetBinding(FrameworkElement.TagProperty, binding);
        }

        private void UpdateValidation(FrameworkElement control, string error)
        {
            var bindingExpression = control.GetBindingExpression(FrameworkElement.TagProperty);

            if (error == null)
            {
                Validation.ClearInvalid(bindingExpression);
            }
            else
            {
                var validationError = new ValidationError(new DataErrorValidationRule(), bindingExpression);
                validationError.ErrorContent = error;
                Validation.MarkInvalid(bindingExpression, validationError);
            }
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
            StaffPage page = new StaffPage(8);
            NavigationService.Navigate(page);
            this.showMessage(StorageUtil.Instance.company.name);
        }

        public void onSuccessRegister()
        {
            this.clearForm(g_register_company);
            this.clearForm(g_register_user);
            this.clearInvalid(g_register_company);
            this.clearInvalid(g_register_user);
            g_register_company.Visibility = Visibility.Hidden;
            g_register_user.Visibility = Visibility.Hidden;
            g_login.Visibility = Visibility.Visible;
            showMessage("register success");
        }

        public void showMessage(string message, float duration = 1.0F)
        {
            //notifier.ShowError(message);
                snackbar.MessageQueue.Enqueue(
                    message,
                    null,
                    null,
                    null,
                    false,
                    true,
                    TimeSpan.FromSeconds(duration));
        }

        private void loginValidate()
        {
            tb_email.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            tb_password.GetBindingExpression(PasswordBoxAssistant.BoundPassword).UpdateSource();
        }

        private bool registerUserValidate()
        {
            tb_register_name.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            tb_register_email.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            tb_register_password.GetBindingExpression(PasswordBoxAssistant.BoundPassword).UpdateSource();
            tb_register_rpassword.GetBindingExpression(PasswordBoxAssistant.BoundPassword).UpdateSource();

            bool name = Validation.GetHasError(tb_register_name);
            bool email = Validation.GetHasError(tb_register_email);
            bool password = Validation.GetHasError(tb_register_password);
            bool c_password = Validation.GetHasError(tb_register_rpassword);


            return name && email && password && c_password;
        }

        private bool registerCompanyValidate()
        {
            tb_register_companyName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            tb_register_address.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            tb_register_phone.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            bool name = Validation.GetHasError(tb_register_companyName);
            bool address = Validation.GetHasError(tb_register_address);
            bool phone = Validation.GetHasError(tb_register_phone);

            return name && address && phone;
        }
        private void clearInvalid(Grid parent)
        {
            foreach (FrameworkElement ctl in parent.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                {
                    Validation.ClearInvalid(((TextBox)ctl).GetBindingExpression(TextBox.TextProperty));
                }
                else if (ctl.GetType() == typeof(PasswordBox))
                {
                    Validation.ClearInvalid(((PasswordBox)ctl).GetBindingExpression(PasswordBoxAssistant.BoundPassword));
                }
            }
        }

        private void clearForm(Grid parent)
        {
            foreach (FrameworkElement ctl in parent.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                {
                    ((TextBox)ctl).Text = string.Empty;
                }
                else if (ctl.GetType() == typeof(PasswordBox))
                {
                    ((PasswordBox)ctl).Password = string.Empty;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            

            // open file dialog   
            f.OpenFileDialog open = new f.OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png)|*.jpg; *.jpeg; *.gif; *.pmg";
            if (open.ShowDialog() == f.DialogResult.OK)
            {
                // display image in picture box  
                //new BitmapImage(new Uri(open.FileName));
                var fill = new ImageBrush(new BitmapImage(new Uri(open.FileName)));
            }

        }

        private void bt_user_image_Click(object sender, RoutedEventArgs e)
        {
            f.OpenFileDialog temp = new f.OpenFileDialog();
            temp.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (temp.ShowDialog() == f.DialogResult.OK)
            {
                tb_user_image.Text = temp.FileName;
                uImage = temp.FileName;
            }
        }

        private void bt_company_image_Click(object sender, RoutedEventArgs e)
        {
            f.OpenFileDialog temp = new f.OpenFileDialog();
            temp.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (temp.ShowDialog() == f.DialogResult.OK)
            {
                tb_company_image.Text = temp.FileName;
                cImage = temp.FileName;
            }
        }
    }
}
