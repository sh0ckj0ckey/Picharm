

namespace 图虫.Models.SitesSearchResult
{
    public class SitesSearchResult
    {
        public string message { get; set; }
        public Data data { get; set; }
        public string result { get; set; }
    }

    public class Data
    {
        public Site_List[] site_list { get; set; }
        public int sites { get; set; }
        public string searchId { get; set; }
    }

    public class Site_List
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
        public bool is_following { get; set; }
        public int following { get; set; }
        public string location { get; set; }
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

namespace 图虫.Models.TagsSearchResult
{
    public class TagsSearchResult
    {
        public string message { get; set; }
        public Data data { get; set; }
        public string result { get; set; }
    }

    public class Data
    {
        public Tag_List[] tag_list { get; set; }
        public int tags { get; set; }
        public string searchId { get; set; }
    }

    public class Tag_List
    {
        public string tag_name { get; set; }
        public string url { get; set; }
        public string tag_id { get; set; }
        public Cover_Image cover_image { get; set; }
        public string type { get; set; }
        public string event_type { get; set; }
    }

    public class Cover_Image
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
}

namespace 图虫.Models.PostsSearchResult
{
    //public class PostsSearchResult
    //{
    //    public string result { get; set; }
    //    public Data data { get; set; }
    //}

    //public class Data
    //{
    //    public Post_List[] post_list { get; set; }
    //    public int posts { get; set; }
    //}

    //public class Post_List
    //{
    //    public string post_id { get; set; }
    //    public string author_id { get; set; }
    //    public string type { get; set; }
    //    public string published_at { get; set; }
    //    public string excerpt { get; set; }
    //    public int favorites { get; set; }
    //    public int comments { get; set; }
    //    public string title { get; set; }
    //    public int image_count { get; set; }
    //    public bool rewardable { get; set; }
    //    public int rewards { get; set; }
    //    public bool wallpaper { get; set; }
    //    public int views { get; set; }
    //    public bool collected { get; set; }
    //    public int downloads { get; set; }
    //    public bool delete { get; set; }
    //    public bool update { get; set; }
    //    public string url { get; set; }
    //    public bool recommend { get; set; }
    //    public int is_self { get; set; }
    //    public Site site { get; set; }
    //    public bool is_favorite { get; set; }
    //    public Image[] images { get; set; }
    //    public Title_Image title_image { get; set; }
    //    public string content { get; set; }
    //    public int shares { get; set; }
    //    public int collect_num { get; set; }
    //    public Tag[] tags { get; set; }
    //    public bool is_top { get; set; }
    //}

    //public class Site
    //{
    //    public string site_id { get; set; }
    //    public string type { get; set; }
    //    public string name { get; set; }
    //    public string description { get; set; }
    //    public string icon { get; set; }
    //    public int verifications { get; set; }
    //    public object verification_list { get; set; }
    //    public bool verified { get; set; }
    //    public int verified_type { get; set; }
    //    public string verified_reason { get; set; }
    //    public bool is_following { get; set; }
    //    public bool is_follower { get; set; }
    //}

    //public class Title_Image
    //{
    //    public int width { get; set; }
    //    public int height { get; set; }
    //    public int img_id { get; set; }
    //    public string url { get; set; }
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
    //    public bool isAuthorTK { get; set; }
    //}

    //public class Source
    //{
    //    public string f { get; set; }
    //    public string g { get; set; }
    //    public string l { get; set; }
    //    public string lr { get; set; }
    //    public string m { get; set; }
    //    public string mr { get; set; }
    //    public string s { get; set; }
    //    public string t { get; set; }
    //}

    //public class Tag
    //{
    //    public string tag_id { get; set; }
    //    public string tag_name { get; set; }
    //    public string type { get; set; }
    //    public string event_type { get; set; }
    //    public string status { get; set; }
    //    public string title { get; set; }
    //    public string sub_title { get; set; }
    //    public string description { get; set; }
    //    public string cover_img_id { get; set; }
    //    public int acl { get; set; }
    //    public string acl_desc { get; set; }
    //    public string created_at { get; set; }
    //}


    public class PostsSearchResult
    {
        public string result { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Post_List[] post_list { get; set; }
        public int posts { get; set; }
    }

    public class Post_List
    {
        public string post_id { get; set; }
        public string author_id { get; set; }
        public string type { get; set; }
        public string published_at { get; set; }
        public string excerpt { get; set; }
        public int favorites { get; set; }
        public int comments { get; set; }
        public string title { get; set; }
        public int image_count { get; set; }
        public bool rewardable { get; set; }
        public int rewards { get; set; }
        public bool wallpaper { get; set; }
        public int views { get; set; }
        public bool collected { get; set; }
        public int downloads { get; set; }
        public bool delete { get; set; }
        public bool update { get; set; }
        public string url { get; set; }
        public bool recommend { get; set; }
        public int is_self { get; set; }
        public Site site { get; set; }
        public bool is_favorite { get; set; }
        public Image[] images { get; set; }
        public Title_Image title_image { get; set; }
        public string content { get; set; }
        public int shares { get; set; }
        public int collect_num { get; set; }
        public Tag[] tags { get; set; }
        public bool is_top { get; set; }
    }

    public class Site
    {
        public string site_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public int verifications { get; set; }
        public object verification_list { get; set; }
        public bool verified { get; set; }
        public int verified_type { get; set; }
        public string verified_reason { get; set; }
        public bool is_following { get; set; }
        public bool is_follower { get; set; }
    }

    public class Title_Image
    {
        public int width { get; set; }
        public int height { get; set; }
        public int img_id { get; set; }
        public string url { get; set; }
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
        public bool isAuthorTK { get; set; }
    }

    public class Source
    {
        public string f { get; set; }
        public string g { get; set; }
        public string l { get; set; }
        public string lr { get; set; }
        public string m { get; set; }
        public string mr { get; set; }
        public string s { get; set; }
        public string t { get; set; }
    }

    public class Tag
    {
        public int tag_id { get; set; }
        public string tag_name { get; set; }
        public string type { get; set; }
        public string event_type { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string sub_title { get; set; }
        public string description { get; set; }
        public string cover_img_id { get; set; }
        public string cover_url { get; set; }
        public bool acl { get; set; }
        public string acl_desc { get; set; }
        public string created_at { get; set; }
        public int subscribers { get; set; }
        public int posts { get; set; }
        public int participants { get; set; }
        public bool subscribed { get; set; }
        public object owners { get; set; }
        public object image_urls { get; set; }
        public int apply_status { get; set; }
    }


}
