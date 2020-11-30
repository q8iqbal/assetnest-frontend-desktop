using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Dashboard
{
    class ModelDashboard
    {
        public class ModelDasboard
        {
            public int id { get; set; }
            public int company_id { get; set; }
            public int staff_id { get; set; }
            public int employee_id { get; set; }
            public int admin_id { get; set; }
        }

        public class Dashboard
        {
            public int current_page { get; set; }
            public ModelDashboard dashboard { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public Dashboard dashboard { get; set; }
        }
    }
}
