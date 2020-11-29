using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Model
{
    public class ModelProfile
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object image { get; set; }
        public string role { get; set; }
        public object deleted_at { get; set; }
    }

    public class Profile
    {
        public int current_page { get; set; }
        public ModelProfile profile { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public Profile profile { get; set; }
    }
}
