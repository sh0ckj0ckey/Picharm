

namespace 图虫.Models.MyFollow
{
    public class MyFollow
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
        public string domain { get; set; }
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
        public Verification_List[] verification_list { get; set; }
        public Ac_Camera ac_camera { get; set; }
        public Owner_Tag[] owner_tag { get; set; }
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
        public Adorned adorned { get; set; }
    }

    public class Adorned
    {
        public string id { get; set; }
        public string camera_name { get; set; }
        public string icon_active_url { get; set; }
        public string icon_inactive_url { get; set; }
        public int threshold { get; set; }
        public string locked_text { get; set; }
        public string unlocked_text { get; set; }
        public string obtained_text { get; set; }
        public string to_lock_url { get; set; }
        public string to_lock_text { get; set; }
    }

    public class Verification_List
    {
        public int verification_type { get; set; }
        public string verification_reason { get; set; }
    }

    public class Owner_Tag
    {
        public string tag_id { get; set; }
        public string type { get; set; }
        public string tag_name { get; set; }
        public string event_type { get; set; }
        public string vote { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public string cover_img_id { get; set; }
        public string acl { get; set; }
        public string acl_desc { get; set; }
        public string created_at { get; set; }
    }

}
