using assetnest_wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.ApiResponse
{
    class LoginResponse
    {
        public string token { get; set; }
        public string token_type { get; set; }
        public User user { get; set; }
    }
}
