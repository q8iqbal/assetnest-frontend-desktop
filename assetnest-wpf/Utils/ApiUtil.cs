using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Utils
{
    public sealed class ApiUtil
    {
        private static readonly ApiUtil instance = new ApiUtil();
        public RestClient client { get; }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ApiUtil()
        {
        }

        private ApiUtil()
        {
            client = new RestClient(Constants.BASE_URL);
        }

        public void setToken(String token)
        {
            client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", token));
        }

        public static ApiUtil Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
