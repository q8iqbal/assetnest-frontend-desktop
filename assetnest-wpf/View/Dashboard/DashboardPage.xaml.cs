using assetnest_wpf.View.ListUser;
using assetnest_wpf.Model;
using assetnest_wpf.Utils;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using Velacro.UIElements.Basic;
using Velacro.UIElements.TextBlock;

namespace assetnest_wpf.View.Dashboard
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    ///
    ///

    public partial class DashboardPage : MyPage
    {
        private IMyTextBlock companyNameTxtBlock;
        private IMyTextBlock companyAddressTxtBlock;
        private IMyTextBlock companyDescriptionTxtBlock;
        private IMyTextBlock companyPhoneTxtBlock;
        private Image companyImageImg;
        private IMyTextBlock adminTxtBlock;
        private IMyTextBlock employeeTxtBlock;
        private IMyTextBlock totalTxtBlock;
        private BuilderTextBlock txtBlockBuilder;

        public DashboardPage()
        {
            InitializeComponent();
            setController(new DashboardController(this));
            initUIBuilders();
            initUIElements();
            initDashboard();
        }

        private void initUIBuilders()
        {
            txtBlockBuilder = new BuilderTextBlock();
        }

        private void initUIElements()
        {
            companyNameTxtBlock = txtBlockBuilder.activate(this, "companyname_txt");
            companyAddressTxtBlock = txtBlockBuilder.activate(this, "companyaddress_txt");
            companyDescriptionTxtBlock = txtBlockBuilder.activate(this, "companydescription_txt");
            companyPhoneTxtBlock = txtBlockBuilder.activate(this, "companyphone_txt");
            companyImageImg = this.FindName("companyimage_img") as Image;
            totalTxtBlock = txtBlockBuilder.activate(this, "totalValue_txt");
            adminTxtBlock = txtBlockBuilder.activate(this, "adminValue_txt");
            employeeTxtBlock = txtBlockBuilder.activate(this, "employeeValue_txt");
        }

        private void initDashboard()
        {
            getController().callMethod("getCompany");
            getController().callMethod("getUserTotal", "admin");
            getController().callMethod("getUserTotal", "user");
            getController().callMethod("getTotal");
        }

        public void setCompany(Company company)
        {
            companyNameTxtBlock.setText(company.name);
            companyAddressTxtBlock.setText(company.address);
            companyPhoneTxtBlock.setText(company.phone);
            if (company.description != null)
            {
                companyDescriptionTxtBlock.setText(company.description);
            }
            if (company.image != null)
            {
                companyImageImg.Source 
                    = new BitmapImage(new Uri(Constants.BASE_URL + company.image));
            }
        }

        public void setAdminTotal(int adminTotal)
        {
            this.Dispatcher.Invoke(() => {
                adminTxtBlock.setText(adminTotal.ToString());
            });
        }

        public void setUserTotal(int userTotal)
        {
            this.Dispatcher.Invoke(() => {
                employeeTxtBlock.setText(userTotal.ToString());
            });
        }

        public void setTotal(int total)
        {
            this.Dispatcher.Invoke(() => {
                totalTxtBlock.setText(total.ToString());
            });
        }


        private void btnAllStaff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListUserPage(""));
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListUserPage( "admin"));
        }

        private void btnEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListUserPage( "user"));
        }
    }
}