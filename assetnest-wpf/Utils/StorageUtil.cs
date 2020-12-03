using assetnest_wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.Utils
{
    public sealed class StorageUtil
    {
        private static readonly StorageUtil instance = new StorageUtil();
        public User user { get; set; } 
        public Company company { get; set; }

        static StorageUtil(){}
        private StorageUtil(){}

        public static StorageUtil Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
