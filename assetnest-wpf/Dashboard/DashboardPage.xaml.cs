using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace assetnest_wpf.Dashboard
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    ///
    ///
    
    public partial class DashboardPage : MyPage
    {
        private IMyTextBlock totalTxtBlock;
        private IMyTextBlock adminTxtBlock;
        private IMyTextBlock employeeTxtBlock;
        private BuilderTextBlock txtBlockBuilder;

        public DashboardPage()
        {
            InitializeComponent();
            this.KeepAlive = true;
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
            totalTxtBlock = txtBlockBuilder.activate(this, "totalValue_txt");
            adminTxtBlock = txtBlockBuilder.activate(this, "adminValue_txt");
            employeeTxtBlock = txtBlockBuilder.activate(this, "employeeValue_txt");
        }

        private void initDashboard()
        {
            getController().callMethod("getUserTotal", "admin");
            getController().callMethod("getUserTotal", "user");
            getController().callMethod("getTotal");
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
    }
}
