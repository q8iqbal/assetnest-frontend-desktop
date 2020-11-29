using System;
using System.Collections.Generic;
using System.IO;
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
using Velacro.UIElements.TextBlock;

namespace assetnest_wpf.Profile
{
    public partial class ProfilePage : MyPage
    {
        //private IMyTexBlock userTextBlock;
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

        }
        private void initUIElements()
        {
         
        }
        public void onProfileButtonClick()
        {
            //getController().callMethod("profile", nameValue_txt.Text, roleValue_txt.Text, emailValue_txt.Text);
        }

        public getProfile()
        {
            String token = File.ReadAllText(@"userToken.txt");
            //getController.callMethod("profile", token);
        }
    }
}
