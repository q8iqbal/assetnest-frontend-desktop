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

namespace assetnest_wpf.Profile
{
    public partial class ProfilePage : MyPage
    {
        //private IMyLabel userLabel;
        //private IMyLabel fullnameLabel;
        public ProfilePage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new ProfileController(this));
            initUIBuilders();
            initUIElements();
        }
        private void initUIBuilders()
        {
            //buttonBuilder = new BuilderButton();
        }
        private void initUIElements()
        {
            //profileButton = buttonBuilder
                //.activate(this, "profileButton_btn")
                //.addOnClick(this, "onProfileButtonClick");
            //nameTxtBox = txtBoxBuilder.activate(this, "nameValue_txt");
            //roleTxtBox = txtBoxBuilder.activate(this, "roleValue_txt");
            //emailTxtBox = txtBoxBuilder.activate(this, "emailValue_txt");
        }
        public void onProfileButtonClick()
        {
            //getController().callMethod("profile", nameValue_txt.Text, roleValue_txt.Text, emailValue_txt.Text);
        }
    }
}
