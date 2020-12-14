using System;
using System.IO;
using Velacro.UIElements.Basic;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.Button;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using assetnest_wpf.Login;

namespace assetnest_wpf.Profile
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
        private readonly int _id;
        public ProfilePage(int id)
        {
            _id = id;
            InitializeComponent();
            this.KeepAlive = true;
            setController(new ProfileController(this));
            initUIBuilders();
            initUIElements();
            getProfile();
        }

        public void setProfile(ModelProfile profiles)
        {
            this.Dispatcher.Invoke(() => {
                roleTxtBlock.setText(profiles.role);
                nameTxtBlock.setText(profiles.name);
                nameValueTxtBlock.setText(profiles.name);
                roleValueTxtBlock.setText(profiles.role);
                emailTxtBlock.setText(profiles.email);

                if(profiles.image != null)
                    image.Source = new BitmapImage(new Uri("http://api.assetnest.me" + profiles.image));
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
            String token = File.ReadAllText(@"userToken.txt");
            getController().callMethod("profile", token, _id);
        }

        public void onLogoutClick()
        {
            String token = File.ReadAllText(@"userToken.txt");
            getController().callMethod("logout", token);
        }

        public void navigateToLogin()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.NavigationService.Navigate(new LoginPage());
            });
        }
    }
}
