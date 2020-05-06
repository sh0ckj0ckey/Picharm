

namespace 图虫.Models.MyLike
{
    //public class MyLike
    //{
    //    public int count { get; set; }
    //    public Work_List[] work_list { get; set; }
    //    public string before_timestamp { get; set; }
    //    public bool more { get; set; }
    //    public string result { get; set; }
    //}

    //public class Work_List
    //{
    //    public string type { get; set; }
    //    public Entry entry { get; set; }
    //}

    //public class Entry
    //{
    //    public string post_id { get; set; }
    //    public string author_id { get; set; }
    //    public string type { get; set; }
    //    public string url { get; set; }
    //    public string published_at { get; set; }
    //    public string excerpt { get; set; }
    //    public int favorites { get; set; }
    //    public int comments { get; set; }
    //    public string title { get; set; }
    //    public int image_count { get; set; }
    //    public bool recommend { get; set; }
    //    public int is_self { get; set; }
    //    public bool delete { get; set; }
    //    public bool update { get; set; }
    //    public Image[] images { get; set; }
    //    public object title_image { get; set; }
    //    public Tag[] tags { get; set; }
    //    public Site site { get; set; }
    //    public string created { get; set; }
    //    public bool is_favorite { get; set; }
    //    public object music { get; set; }
    //    public string data_type { get; set; }
    //}

    //public class Site
    //{
    //    public string site_id { get; set; }
    //    public string type { get; set; }
    //    public string name { get; set; }
    //    public string domain { get; set; }
    //    public string description { get; set; }
    //    public int followers { get; set; }
    //    public string url { get; set; }
    //    public string icon { get; set; }
    //    public bool is_bind_everphoto { get; set; }
    //    public bool has_everphoto_note { get; set; }
    //    public bool verified { get; set; }
    //    public int verifications { get; set; }
    //    public Verification_List[] verification_list { get; set; }
    //    public bool is_following { get; set; }
    //    public bool is_follower { get; set; }
    //}

    //public class Verification_List
    //{
    //    public int verification_type { get; set; }
    //    public string verification_reason { get; set; }
    //}

    //public class Image
    //{
    //    public int img_id { get; set; }
    //    public string img_id_str { get; set; }
    //    public int user_id { get; set; }
    //    public string title { get; set; }
    //    public string excerpt { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }
    //    public Source source { get; set; }
    //    public int is_authorized_tc { get; set; }
    //}

    //public class Source
    //{
    //    public string t { get; set; }
    //    public string g { get; set; }
    //    public string s { get; set; }
    //    public string m { get; set; }
    //    public string mr { get; set; }
    //    public string l { get; set; }
    //    public string lr { get; set; }
    //    public string ft640 { get; set; }
    //    public string f { get; set; }
    //}

    //public class Tag
    //{
    //    public int tag_id { get; set; }
    //    public string type { get; set; }
    //    public string tag_name { get; set; }
    //    public string event_type { get; set; }
    //    public string vote { get; set; }
    //    public string status { get; set; }
    //    public string title { get; set; }
    //    public string subtitle { get; set; }
    //    public string description { get; set; }
    //    public string cover_img_id { get; set; }
    //    public string acl { get; set; }
    //    public string acl_desc { get; set; }
    //    public string created_at { get; set; }
    //}



    public class MyLike
    {
        public int count { get; set; }
        public Work_List[] work_list { get; set; }
        public string before_timestamp { get; set; }
        public bool more { get; set; }
        public string result { get; set; }
    }

    public class Work_List
    {
        public string type { get; set; }
        public Entry entry { get; set; }
    }

    public class Entry
    {
        public string post_id { get; set; }
        public string author_id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string published_at { get; set; }
        public string excerpt { get; set; }
        public int favorites { get; set; }
        public object comments { get; set; }
        public string title { get; set; }
        public int image_count { get; set; }
        public bool recommend { get; set; }
        public object is_self { get; set; }
        public bool delete { get; set; }
        public bool update { get; set; }
        public Image[] images { get; set; }
        public object title_image { get; set; }
        public Tag[] tags { get; set; }
        public Site site { get; set; }
        public string created { get; set; }
        public bool is_favorite { get; set; }
        public Music music { get; set; }
        public string data_type { get; set; }
        public string liked_at { get; set; }
        public string vid { get; set; }
        public string video_id { get; set; }
        public string content { get; set; }
        public Author author { get; set; }
        public string cover { get; set; }
        public string raw_cover { get; set; }
        public int views { get; set; }
        public string video_width { get; set; }
        public string video_height { get; set; }
        public string share_url { get; set; }
        public string share_cover { get; set; }
        public string duration { get; set; }
        public bool is_recommend { get; set; }
        public string passed_time { get; set; }
        public bool is_ultra { get; set; }
        public object[] category { get; set; }
        public bool collected { get; set; }
        public int collect_num { get; set; }
        public int shares { get; set; }
        public Cover_Detail cover_detail { get; set; }
        public bool liked { get; set; }
        public bool is_following { get; set; }
        public bool is_top { get; set; }
        public object[] topics { get; set; }
        public string video_model { get; set; }
    }

    public class Site
    {
        public string site_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public string description { get; set; }
        public int followers { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public bool is_bind_everphoto { get; set; }
        public bool has_everphoto_note { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public Verification_List[] verification_list { get; set; }
        public bool is_following { get; set; }
        public bool is_follower { get; set; }
    }

    public class Verification_List
    {
        public int verification_type { get; set; }
        public string verification_reason { get; set; }
    }

    public class Music
    {
        public string music_id { get; set; }
        public string common_music_id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string duration { get; set; }
        public string status { get; set; }
        public string url { get; set; }
        public string cover_url { get; set; }
        public string desc { get; set; }
        public float real_duration { get; set; }
        public string hash_tag { get; set; }
        public string suggest_count { get; set; }
        public string recommend_url { get; set; }
        public int tmpl_id { get; set; }
        public int ori_tmpl_id { get; set; }
        public bool is_common_tmpl { get; set; }
    }

    public class Author
    {
        public string site_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public string description { get; set; }
        public int followers { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public bool is_bind_everphoto { get; set; }
        public bool has_everphoto_note { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public object[] verification_list { get; set; }
        public Wallpaper wallpaper { get; set; }
        public Live_Wallpaper live_wallpaper { get; set; }
        public bool is_following { get; set; }
    }

    public class Wallpaper
    {
        public bool wallpaper_following { get; set; }
        public bool wallpaper_reward { get; set; }
    }

    public class Live_Wallpaper
    {
        public bool live_wallpaper_following { get; set; }
        public bool live_wallpaper_reward { get; set; }
    }

    public class Cover_Detail
    {
        public string img_id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Image
    {
        public int img_id { get; set; }
        public string img_id_str { get; set; }
        public int user_id { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Source source { get; set; }
        public int is_authorized_tc { get; set; }
    }

    public class Source
    {
        public string t { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string m { get; set; }
        public string mr { get; set; }
        public string l { get; set; }
        public string lr { get; set; }
        public string ft640 { get; set; }
        public string f { get; set; }
    }

    public class Tag
    {
        public int tag_id { get; set; }
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
