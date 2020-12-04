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

namespace assetnest_wpf.ListUser
{
    /// <summary>
    /// Interaction logic for ListUserPage.xaml
    /// </summary>
    public partial class ListUserPage : MyPage
    {
        public ListUserPage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            
            setController(new ListUserController(this));
            String token = File.ReadAllText(@"userToken.txt");
            getController().callMethod("getUser", token);
        }
    }
}
