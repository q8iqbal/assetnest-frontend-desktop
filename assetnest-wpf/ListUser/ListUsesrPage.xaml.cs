using assetnest_wpf.Profile;
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
        String filter = "&filter";
        String page = "?page=";
        String key = "";
        String value = "";
        int currentPage = 1;
        int maxPage;
        String currentQueryPage = "";
        String currentQueryFilter = "";
        String currentQuery = "";
        String token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MDg0MjAxLCJuYmYiOjE2MDcwODQyMDEsImp0aSI6Im9SWjNCVmFpTDNWb1BKVTYiLCJzdWIiOjYsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.dn197l2g5i4uLzVx49_HLD1jLRJXPvVpctYtF8gcRNI";
        private readonly Frame mainFrame;

        public ListUserPage(Frame frame, String type)
        {
            mainFrame = frame;
            InitializeComponent();
            this.KeepAlive = true;
            if (!type.Equals(""))
            {
                value = type;
                key = "[role]=";
                currentQueryFilter = filter + key + value;
                currentQuery = page + currentPage + currentQueryFilter;
            }
            setController(new ListUserController(this));
            getController().callMethod("getUser", token, currentQuery);

        }
        public void testList(Data data)
        {
            List<Datum> list = data.data;
            maxPage = data.last_page;
            this.Dispatcher.Invoke(() =>
            {
                ListViewUsers.ItemsSource = list;
            });

        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < maxPage)
            {
                currentPage++;
                currentQueryPage = page + currentPage;
                currentQuery = currentQueryPage + currentQueryFilter;
                getController().callMethod("getUser", token, currentQuery);
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                currentQueryPage = page + currentPage;
                currentQuery = currentQueryPage + currentQueryFilter;
                getController().callMethod("getUser", token, currentQuery);
            }
        }

        private void btnAllStaff_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            currentQuery = page + currentPage;
            currentQueryFilter = "";
            getController().callMethod("getUser", token, currentQuery);
        }

        private void btnStaffAdmin_Click(object sender, RoutedEventArgs e)
        {
            key = "[role]=";
            value = "admin";
            currentQueryFilter = filter + key + value;
            currentPage = 1;
            currentQuery = page + currentPage + currentQueryFilter;
            getController().callMethod("getUser", token, currentQuery);
        }

        private void btnStaffEmployee_Click(object sender, RoutedEventArgs e)
        {
            key = "[role]=";
            value = "user";
            currentQueryFilter = filter + key + value;
            currentPage = 1;
            currentQuery = page + currentPage + currentQueryFilter;
            getController().callMethod("getUser", token, currentQuery);
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            key = "[name]=";
            value = tbSearch.Text;
            currentQueryFilter = filter + key + value;
            currentPage = 1;
            currentQuery = page + currentPage + currentQueryFilter;
            getController().callMethod("getUser", token, currentQuery);
        }

        private void itemUser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            Datum item = sp.DataContext as Datum;
            //ListUserPage.Navi
            mainFrame.Navigate(new ProfilePage(item.id));
            //MessageBox.Show("id" + item.id);
        }
    }
}