

namespace 图虫.Models.Discover
{
    public class Discover
    {
        public Banner[] banners { get; set; }
        public Hotevent[] hotEvents { get; set; }
        public Category[] categories { get; set; }
        public string result { get; set; }
    }

    public class Banner
    {
        public string url { get; set; }
        public string src { get; set; }
    }

    public class Hotevent
    {
        public string tag_id { get; set; }
        public string tag_name { get; set; }
        public string created_at { get; set; }
        public string status { get; set; }
        public string posts { get; set; }
        public string new_posts { get; set; }
        public string participants { get; set; }
        public string end_at { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string event_type { get; set; }
        public int image_count { get; set; }
        public string deadline { get; set; }
        public string prize_desc { get; set; }
        public string prize_url { get; set; }
        public string introduction_url { get; set; }
        public int introduction_id { get; set; }
        public int competition_type { get; set; }
        public object[] category { get; set; }
        public string auth_type { get; set; }
        public int remainingDays { get; set; }
        public int dueDays { get; set; }
        public object[] image_posts { get; set; }
        public bool list_banner { get; set; }
        public string[] images { get; set; }
        public string app_url { get; set; }
    }

    public class Category
    {
        public string tag_name { get; set; }
        public int tag_id { get; set; }
    }

}
