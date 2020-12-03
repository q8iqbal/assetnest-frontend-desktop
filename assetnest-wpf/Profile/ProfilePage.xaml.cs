using System;
using System.IO;
using Velacro.UIElements.Basic;
using Velacro.UIElements.TextBlock;

namespace assetnest_wpf.Profile
{
    public partial class ProfilePage : MyPage
    {
        private IMyTextBlock roleTxtBlock;
        private IMyTextBlock nameTxtBlock;
        private IMyTextBlock nameValueTxtBlock; 
        private IMyTextBlock roleValueTxtBlock;
        private IMyTextBlock emailTxtBlock; 
        private BuilderTextBlock txtBlockBuilder;

        public ProfilePage()
        {
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
            });
        }

        private void initUIBuilders()
        {
            txtBlockBuilder = new BuilderTextBlock();
        }

        private void initUIElements()
        {
            roleTxtBlock = txtBlockBuilder.activate(this, "roleText");
            nameTxtBlock = txtBlockBuilder.activate(this, "nameText");
            nameValueTxtBlock = txtBlockBuilder.activate(this, "nameValueText");
            roleValueTxtBlock = txtBlockBuilder.activate(this, "roleValueText");
            emailTxtBlock = txtBlockBuilder.activate(this, "emailValueText");
        }

        private void getProfile()
        {
            String token = File.ReadAllText(@"userToken.txt");
            getController().callMethod("profile", token);
        }
    }
}
