using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Profile
{
    public class ModelProfile
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public string role { get; set; }
        public object deleted_at { get; set; }
    }

    public class RootModelProfile
    {
        public string status { get; set; }
        public ModelProfile data { get; set; }
    }
}
