using System;
using System.IO;
using Velacro.UIElements.Basic;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.Button;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using assetnest_wpf.View.Auth;
using assetnest_wpf.Utils;
using assetnest_wpf.Model;

namespace assetnest_wpf.View.Profile
{
    public partial class ProfilePage : MyPage
    {
        private IMyTextBlock roleTxtBlock;
        private IMyTextBlock nameTxtBlock;
        private IMyTextBlock nameValueTxtBlock; 
        private IMyTextBlock roleValueTxtBlock;
        private IMyTextBlock emailTxtBlock;
        private IMyButton buttonLogout;
        private Image image;
        private BuilderTextBlock txtBlockBuilder;
        private BuilderButton buttonBuilder;
        public ProfilePage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new ProfileController(this));
            initUIBuilders();
            initUIElements();
            getProfile();
        }

        public void setProfile(User profiles)
        {
            this.Dispatcher.Invoke(() => {
                roleTxtBlock.setText(profiles.role);
                nameTxtBlock.setText(profiles.name);
                nameValueTxtBlock.setText(profiles.name);
                roleValueTxtBlock.setText(profiles.role);
                emailTxtBlock.setText(profiles.email);

                if(profiles.image != null)
                    image.Source = new BitmapImage(new Uri(Constants.BASE_URL + profiles.image));
            });
        }

        private void initUIBuilders()
        {
            txtBlockBuilder = new BuilderTextBlock();
            buttonBuilder = new BuilderButton();
            image = new Image();
        }

        private void initUIElements()
        {
            roleTxtBlock = txtBlockBuilder.activate(this, "roleText");
            nameTxtBlock = txtBlockBuilder.activate(this, "nameText");
            nameValueTxtBlock = txtBlockBuilder.activate(this, "nameValueText");
            roleValueTxtBlock = txtBlockBuilder.activate(this, "roleValueText");
            emailTxtBlock = txtBlockBuilder.activate(this, "emailValueText");
            image = this.FindName("user_img") as Image;
            buttonLogout = this.buttonBuilder.activate(this, "btn_logout").addOnClick(this, "onLogoutClick");
        }

        private void getProfile()
        {
            getController().callMethod("profile");
        }

        public void onLogoutClick()
        {
            getController().callMethod("logout");
        }

        public void navigateToLogin()
        {
            this.Dispatcher.Invoke(() =>
            {
                ((MainWindow)MyWindow.GetWindow(this)).mainFrame.Navigate(new AuthPage());
            });
        }
    }
}