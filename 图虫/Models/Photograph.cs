

namespace 图虫.Models.Photograph
{
    public class Photograph
    {
        public Post_List[] post_list { get; set; }
        public string before_timestamp { get; set; }
        public bool more { get; set; }
        public int counts { get; set; }
        public string result { get; set; }
    }

    public class Post_List
    {
        public string post_id { get; set; }
        public string author_id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string published_at { get; set; }
        public string excerpt { get; set; }
        public double favorites { get; set; }
        public double comments { get; set; }
        public string title { get; set; }
        public double image_count { get; set; }
        public bool delete { get; set; }
        public bool update { get; set; }
        public Image[] images { get; set; }
        public object title_image { get; set; }
        public Tag[] tags { get; set; }
        public Site site { get; set; }
        public bool is_favorite { get; set; }
        public object music { get; set; }
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
        public int verifications { get; set; }
        public Verification_List[] verification_list { get; set; }
        public bool verified { get; set; }
        public string verified_type { get; set; }
        public string verified_reason { get; set; }
        public bool is_following { get; set; }
        public bool is_follower { get; set; }
    }

    public class Verification_List
    {
        public string verification_type { get; set; }
        public string verification_reason { get; set; }
    }

    public class Image
    {
        public string img_id { get; set; }
        public string user_id { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Source source { get; set; }
        public string is_authorized_tc { get; set; }
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
    }

    public class Tag
    {
        public string tag_id { get; set; }
        public string type { get; set; }
        public string tag_name { get; set; }
        public string event_type { get; set; }
        public string vote { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string cover_img_id { get; set; }
    }

}
