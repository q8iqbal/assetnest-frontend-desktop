using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assetnest_wpf.ListUser
{
    class ModelListUser
    {


    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Datum
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object image { get; set; }
        public string role { get; set; }
        public object deleted_at { get; set; }
    }

    public class Link
    {
        public string url { get; set; }
        public object label { get; set; }
        public bool active { get; set; }
    }

    public class Data
    {
        public int current_page { get; set; }
        public List<Datum> data { get; set; }
        public string first_page_url { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public string last_page_url { get; set; }
        public List<Link> links { get; set; }
        public string next_page_url { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public object prev_page_url { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public Data data { get; set; }
    }

}