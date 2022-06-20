using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbotSpider.Crawlers.Toutiao
{

    public class Rootobject
    {
        public Datum[] data { get; set; }
        public string impr_id { get; set; }
        public string status { get; set; }
    }

    public class Datum
    {
        public long ClusterId { get; set; }
        public string Title { get; set; }
        public string LabelUrl { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string HotValue { get; set; }
        public string Schema { get; set; }
        public Labeluri LabelUri { get; set; }
        public string ClusterIdStr { get; set; }
        public int ClusterType { get; set; }
        public string QueryWord { get; set; }
        public Image Image { get; set; }
        public string LabelDesc { get; set; }
        public string[] InterestCategory { get; set; }
    }

    public class Labeluri
    {
        public string uri { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Url_List[] url_list { get; set; }
        public int image_type { get; set; }
    }

    public class Url_List
    {
        public string url { get; set; }
    }

    public class Image
    {
        public string uri { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Url_List1[] url_list { get; set; }
        public int image_type { get; set; }
    }

    public class Url_List1
    {
        public string url { get; set; }
    }


}
