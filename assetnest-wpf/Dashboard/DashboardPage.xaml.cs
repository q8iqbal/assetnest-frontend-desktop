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
    public class TodoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class DashboardPage : MyPage
    {
        private IMyTextBlock totalTxtBlock;
        private IMyTextBlock adminTxtBlock;
        private IMyTextBlock employeeTxtBlock;
        private BuilderTextBlock txtBlockBuilder;
        private String token;
        public DashboardPage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new DashboardController(this));
            initUIBuilders();
            initUIElements();
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

    }
}
