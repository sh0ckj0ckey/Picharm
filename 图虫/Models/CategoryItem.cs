

namespace 图虫.Models.CategoryItem
{
    public class CategoryItem
    {
        public Post_List[] post_list { get; set; }
        public string result { get; set; }
    }

    public class Post_List
    {
        public int post_id { get; set; }
        public string author_id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
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
        public int shares { get; set; }
        public int collect_num { get; set; }
        public bool delete { get; set; }
        public bool recommend { get; set; }
        public int is_self { get; set; }
        public string content { get; set; }
        public bool update { get; set; }
        public Image[] images { get; set; }
        public Equip equip { get; set; }
        public Title_Image title_image { get; set; }
        public Tag[] tags { get; set; }
        public Topic[] topics { get; set; }
        public Comment_List[] comment_list { get; set; }
        public User[] users { get; set; }
        public Site site { get; set; }
        public bool is_favorite { get; set; }
        public bool is_top { get; set; }
        public string rqt_id { get; set; }
    }

    public class Equip
    {
        public string display_name { get; set; }
        public string equip_id { get; set; }
    }

    public class Title_Image
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public int img_id { get; set; }
    }

    public class Site
    {
        public int site_id { get; set; }
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
        public string recommend_reason { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public Verification_List[] verification_list { get; set; }
        public Ac_Camera ac_camera { get; set; }
        public Owner_Tag[] owner_tag { get; set; }
        public bool is_following { get; set; }
        public bool is_follower { get; set; }
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

    public class Image
    {
        public int img_id { get; set; }
        public string img_id_str { get; set; }
        public int user_id { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string description { get; set; }
        public bool isAuthorTK { get; set; }
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

    public class Topic
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
        public bool is_owner { get; set; }
        public bool is_top { get; set; }
        public bool is_excellent { get; set; }
        public int participants { get; set; }
    }

    public class Comment_List
    {
        public int note_id { get; set; }
        public Author author { get; set; }
        public string content { get; set; }
        public int sub_notes_count { get; set; }
        public bool liked { get; set; }
        public int likes { get; set; }
        public string created_at { get; set; }
        public Last_Replied last_replied { get; set; }
        public Image1[] images { get; set; }
        public string sort_weight { get; set; }
        public string note_type { get; set; }
        public string author_id { get; set; }
        public bool author_liked { get; set; }
        public Sub_Notes[] sub_notes { get; set; }
    }

    public class Author
    {
        public object site_id { get; set; }
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
    }

    public class Last_Replied
    {
        public object site_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public string intro { get; set; }
        public int posts { get; set; }
        public int followers { get; set; }
        public string recommend_reason { get; set; }
    }

    public class Image1
    {
        public string img_id { get; set; }
        public string img_id_str { get; set; }
        public int user_id { get; set; }
        public string title { get; set; }
        public Source source { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string ext { get; set; }
        public string qualified { get; set; }
    }

    public class Source
    {
        public string t { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string m { get; set; }
        public string mr { get; set; }
        public string l { get; set; }
        public string f { get; set; }
        public string o { get; set; }
    }

    public class Sub_Notes
    {
        public int note_id { get; set; }
        public Author1 author { get; set; }
        public string content { get; set; }
        public int sub_notes_count { get; set; }
        public bool liked { get; set; }
        public int likes { get; set; }
        public string created_at { get; set; }
        public object last_replied { get; set; }
        public object[] images { get; set; }
        public string sort_weight { get; set; }
        public string note_type { get; set; }
        public string author_id { get; set; }
        public bool author_liked { get; set; }
    }

    public class Author1
    {
        public int site_id { get; set; }
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
        public Verification_List1[] verification_list { get; set; }
    }

    public class Verification_List1
    {
        public int verification_type { get; set; }
        public string verification_reason { get; set; }
    }

    public class User
    {
        public object site_id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
    }

}
