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
            String token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MDg0MjAxLCJuYmYiOjE2MDcwODQyMDEsImp0aSI6Im9SWjNCVmFpTDNWb1BKVTYiLCJzdWIiOjYsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.dn197l2g5i4uLzVx49_HLD1jLRJXPvVpctYtF8gcRNI";
            getController().callMethod("getUser", token);
            
        }
        public void testList(List<Datum> list) {
            this.Dispatcher.Invoke(() => {
                ListViewProducts.ItemsSource = list;
            });
                
        }
    }
}
