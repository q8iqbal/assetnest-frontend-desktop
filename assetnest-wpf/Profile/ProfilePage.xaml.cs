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
using assetnest_wpf.Model;

namespace assetnest_wpf.Profile
{
    public partial class ProfilePage : MyPage
    {

        public ProfilePage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new ProfileController(this));
            initUIBuilders();
            initUIElements();
            getProfile();
        }
        
        public void setProfile(List<ModelProfile> profiles)
        {
            this.Dispatcher.Invoke(() =>
            {
                profileData.ItemsSource = profiles;
            });
        }
        
        private void initUIBuilders()
        {
            
        }
        
        private void initUIElements()
        {
            
        }
        private void getProfile()
        {
            String token = File.ReadAllText(@"userToken.txt");
            getController().callMethod("profile", token);
        }
    }
}
