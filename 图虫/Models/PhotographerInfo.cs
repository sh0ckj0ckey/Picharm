

namespace 图虫.Models.Photographer
{
    public class PhotographerInfo
    {
        public Site site { get; set; }
        public Cover cover { get; set; }
        public Statistics statistics { get; set; }
        public Relationship relationship { get; set; }
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
        public int followers { get; set; }
        public int members { get; set; }
        public int group_posts { get; set; }
        public string recommend_reason { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public Verification_List[] verification_list { get; set; }
        public int point_level { get; set; }
        public bool is_following { get; set; }
        public bool is_follower { get; set; }
        public Ac_Camera ac_camera { get; set; }
        public object[] owner_tag { get; set; }
        public bool qualified { get; set; }
        public int following { get; set; }
        public int favorites { get; set; }
        public string user_gender { get; set; }
        public string user_location { get; set; }
        public object[] tags { get; set; }
        public string cover_url { get; set; }
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

    public class Cover
    {
        public string post_id { get; set; }
        public string[] images { get; set; }
        public Size[] sizes { get; set; }
    }

    public class Size
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Statistics
    {
        public int works { get; set; }
        public int favorites { get; set; }
        public int events { get; set; }
        public int tags { get; set; }
    }

    public class Relationship
    {
        public string to_id { get; set; }
        public int is_following { get; set; }
    }

}
