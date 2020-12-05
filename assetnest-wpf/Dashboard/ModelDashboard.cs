using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Dashboard
{
    public class ModelDashboard : INotifyPropertyChanged
    {
        private int totalValue_txt;
        private int adminValue_txt;
        private int employeeValue_txt;

        public int AdminValue_txt
        {
            get { return adminValue_txt; }
            set
            {
                if(adminValue_txt != value)
                {
                    adminValue_txt = value;
                    OnPropertyChanged();
                }
            }
        }

        public int EmployeeValue_txt
        {
            get { return employeeValue_txt; }
            set
            {
                if(employeeValue_txt != value)
                {
                    employeeValue_txt = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalValue_txt
        {
            get { return totalValue_txt = adminValue_txt + employeeValue_txt; }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
