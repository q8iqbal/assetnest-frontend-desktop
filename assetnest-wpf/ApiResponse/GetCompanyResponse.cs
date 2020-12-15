using assetnest_wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.ApiResponse
{
    class GetCompanyResponse
    {
        public string status { get; set; }
        public Company data { get; set; }
    }
}
