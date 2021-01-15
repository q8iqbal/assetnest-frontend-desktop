using assetnest_wpf.Utils;
using assetnest_wpf.View.AddStaff;
using assetnest_wpf.View.Profile;
using assetnest_wpf.View.Staff;
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

namespace assetnest_wpf.View.ListUser
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

        public ListUserPage(String type)
        {
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
            getController().callMethod("getUser", currentQuery);

        }
        public void showList(Data data)
        {   
            List<Datum> list = new List<Datum>();
            if (data.data != null) { 
                list = data.data;
                maxPage = data.last_page;
                foreach (Datum datum in list) {
                    if (datum.image == null)
                    {
                        datum.image = "../../Assets/profile.png";
                    }
                    else
                    {
                        datum.image = Constants.BASE_URL + datum.image;
                    }

                    if (datum.role == "admin") {
                        datum.role = "Admin";
                    }
                    else
                    {
                        datum.role = "Employee";
                    }
                }
            }
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
                getController().callMethod("getUser", currentQuery);
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                currentQueryPage = page + currentPage;
                currentQuery = currentQueryPage + currentQueryFilter;
                getController().callMethod("getUser", currentQuery);
            }
        }

        private void btnAllStaff_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            currentQuery = page + currentPage;
            currentQueryFilter = "";
            getController().callMethod("getUser", currentQuery);
        }

        private void btnStaffAdmin_Click(object sender, RoutedEventArgs e)
        {
            key = "[role]=";
            value = "admin";
            currentQueryFilter = filter + key + value;
            currentPage = 1;
            currentQuery = page + currentPage + currentQueryFilter;
            getController().callMethod("getUser", currentQuery);
        }

        private void btnStaffEmployee_Click(object sender, RoutedEventArgs e)
        {
            key = "[role]=";
            value = "user";
            currentQueryFilter = filter + key + value;
            currentPage = 1;
            currentQuery = page + currentPage + currentQueryFilter;
            getController().callMethod("getUser", currentQuery);
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            key = "[name]=";
            value = tbSearch.Text;
            currentQueryFilter = filter + key + value;
            currentPage = 1;
            currentQuery = page + currentPage + currentQueryFilter;
            getController().callMethod("getUser", currentQuery);
        }

        private void itemUser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            Datum item = sp.DataContext as Datum;
            //ListUserPage.Navi
            NavigationService.Navigate(new StaffPage(item.id));
            //MessageBox.Show("id" + item.id);
        }

        private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddStaffPage());
        }
    }
}