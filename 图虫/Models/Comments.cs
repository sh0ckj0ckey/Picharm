
namespace 图虫.Models.Comments
{
    public class Comments
    {
        public int before_timestamp { get; set; }
        public string comments { get; set; }
        public string parent_comments { get; set; }
        public bool more { get; set; }
        public Commentlist[] commentlist { get; set; }
        public string baseUrl { get; set; }
        public string result { get; set; }
    }

    public class Commentlist
    {
        public string note_id { get; set; }
        public string post_id { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }
        public bool delete { get; set; }
        public bool reply { get; set; }
        public string author_id { get; set; }
        public int anonymous { get; set; }
        public int likes { get; set; }
        public Sub_Notes[] sub_notes { get; set; }
        public int sub_notes_count { get; set; }
        public Last_Replied last_replied { get; set; }
        public string parent_note_id { get; set; }
        public string reply_to_note_id { get; set; }
        public object[] images { get; set; }
        public string sort_weight { get; set; }
        public string note_type { get; set; }
        public bool author_liked { get; set; }
        public object reply_to { get; set; }
        public Author author { get; set; }
        public bool is_best { get; set; }
    }

    public class Last_Replied
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
    }

    public class Verification_List
    {
        public int verification_type { get; set; }
        public string verification_reason { get; set; }
    }

    public class Author
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
        public Appearance appearance { get; set; }
        public bool is_bind_everphoto { get; set; }
        public bool has_everphoto_note { get; set; }
        public int followers { get; set; }
        public string recommend_reason { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public object[] verification_list { get; set; }
        public Ac_Camera ac_camera { get; set; }
        public object[] owner_tag { get; set; }
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

    public class Sub_Notes
    {
        public string note_id { get; set; }
        public string post_id { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }
        public bool delete { get; set; }
        public bool reply { get; set; }
        public string author_id { get; set; }
        public int anonymous { get; set; }
        public int likes { get; set; }
        public object[] sub_notes { get; set; }
        public int sub_notes_count { get; set; }
        public object last_replied { get; set; }
        public string parent_note_id { get; set; }
        public string reply_to_note_id { get; set; }
        public object[] images { get; set; }
        public string sort_weight { get; set; }
        public string note_type { get; set; }
        public bool author_liked { get; set; }
        public string[] reply_to_array { get; set; }
        public Reply_To[] reply_to { get; set; }
        public Author1 author { get; set; }
    }

    public class Author1
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
        public Appearance1 appearance { get; set; }
        public bool is_bind_everphoto { get; set; }
        public bool has_everphoto_note { get; set; }
        public int followers { get; set; }
        public string recommend_reason { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public Verification_List1[] verification_list { get; set; }
        public Ac_Camera1 ac_camera { get; set; }
        public object[] owner_tag { get; set; }
    }

    public class Appearance1
    {
        public string color { get; set; }
        public string image { get; set; }
    }

    public class Ac_Camera1
    {
        public int obtained_num { get; set; }
        public object adorned { get; set; }
    }

    public class Verification_List1
    {
        public int verification_type { get; set; }
        public string verification_reason { get; set; }
    }

    public class Reply_To
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
        public Appearance2 appearance { get; set; }
        public bool is_bind_everphoto { get; set; }
        public bool has_everphoto_note { get; set; }
        public int followers { get; set; }
        public string recommend_reason { get; set; }
        public bool verified { get; set; }
        public int verifications { get; set; }
        public object[] verification_list { get; set; }
        public Ac_Camera2 ac_camera { get; set; }
        public object[] owner_tag { get; set; }
    }

    public class Appearance2
    {
        public string color { get; set; }
        public string image { get; set; }
    }

    public class Ac_Camera2
    {
        public int obtained_num { get; set; }
        public object adorned { get; set; }
    }

}
