

namespace 图虫.Models.MyFans
{
    public class MyFans
    {
        public Site[] sites { get; set; }
        public string before_timestamp { get; set; }
        public bool more { get; set; }
        public string result { get; set; }
    }

    public class Site
    {
        public string site_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public object domain { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public string intro { get; set; }
        public int posts { get; set; }
        public Appearance appearance { get; set; }
        public bool is_bind_everphoto { get; set; }
        public bool has_everphoto_note { get; set; }
        public int followers { get; set; }
        public int members { get; set; }
        public int group_posts { get; set; }
        public string recommend_reason { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public object[] verification_list { get; set; }
        public Ac_Camera ac_camera { get; set; }
        public object[] owner_tag { get; set; }
        public int following { get; set; }
        public string location { get; set; }
        public bool is_following { get; set; }
    }

    public class Appearance
    {
        public string color { get; set; }
        public string image { get; set; }
    }

    public class Ac_Camera
    {
        public int obtained_num { get; set; }
        public object adorned { get; set; }
    }

}
