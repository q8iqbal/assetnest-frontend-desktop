using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Model
{
    public class User
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public string role { get; set; }
        public string password { get; set; }
    }
}
