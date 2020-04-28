using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// Added during quickstart
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Web.Syndication;

namespace BackgroundTasks
{
    public sealed class LiveTileTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            var feed = await GetFeed();

            // Update the live tile with the feed items.
            if (feed != null)
            {
                UpdateTile(feed);
            }

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static async Task<FeedVM> GetFeed()
        {
            try
            {
                string url = "https://api.tuchong.com/2/feed-app?page=1&type=refresh";
                using (HttpClient http = new HttpClient())
                {
                    var response = await http.GetAsync(new Uri(url));
                    var jsonMessage = await response.Content.ReadAsStringAsync();

                    JsonSerializerSettings jss = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    Feeds feed = JsonConvert.DeserializeObject<Feeds>(jsonMessage, jss);

                    if (feed == null || feed.feedList.Length <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        int index = 2;
                        string lastpost = "";
                        List<Feedlist> temp = new List<Feedlist>();
                        foreach (var item in feed.feedList)
                        {
                            if (item.type == "post")
                            {
                                temp.Add(item);
                            }
                            if (item.entry.post_id != 0)
                            {
                                lastpost = item.entry.post_id.ToString();
                            }
                            else if (item.entry.vid != 0)
                            {
                                lastpost = item.entry.vid.ToString();
                            }
                        }
                        // 如果没有获取到一个post类型，就继续请求新数据，直到找到一个数据
                        while (temp.Count == 0)
                        {
                            response = await http.GetAsync(new Uri("https://api.tuchong.com/2/feed-app?" + "page=" + index + "&post_id=" + lastpost + "&type=loadmore"));
                            index++;
                            jsonMessage = await response.Content.ReadAsStringAsync();
                            feed = JsonConvert.DeserializeObject<Feeds>(jsonMessage, jss);
                            if (feed == null || feed.feedList.Length <= 0)
                            {
                                return null;
                            }
                            foreach (var item in feed.feedList)
                            {
                                if (item.type == "post")
                                {
                                    temp.Add(item);
                                }
                                if (item.entry.post_id != 0)
                                {
                                    lastpost = item.entry.post_id.ToString();
                                }
                                else if (item.entry.vid != 0)
                                {
                                    lastpost = item.entry.vid.ToString();
                                }
                            }
                        }
                        var ret = new FeedVM(temp[new Random().Next(0, temp.Count)]);
                        return ret;
                    }
                }
            }
            catch { return null; }
        }

        private static void UpdateTile(FeedVM feed)
        {
            try
            {
                TileContent content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = new TileBinding()
                        {
                            Branding = TileBranding.Name,
                            Content = new TileBindingContentAdaptive()
                            {
                                PeekImage = new TilePeekImage()
                                {
                                    Source = feed.Cover,
                                    AlternateText = "加载图片中..."
                                },
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = feed.Name,
                                        HintStyle = AdaptiveTextStyle.Body
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = feed.Date,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintWrap = true
                                    }
                                }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                PeekImage = new TilePeekImage()
                                {
                                    Source = feed.Cover,
                                    AlternateText = "加载图片中..."
                                },
                                TextStacking = TileTextStacking.Top,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = feed.Name,
                                        HintStyle = AdaptiveTextStyle.Base
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = feed.Desc,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintWrap=true,
                                        HintMaxLines=2
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = feed.Date,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintWrap=true
                                    },
                                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                PeekImage = new TilePeekImage()
                                {
                                    Source = feed.Cover,
                                    AlternateText = "加载图片中...",
                                },
                                TextStacking = TileTextStacking.Top,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text =feed.Name,
                                        HintStyle=AdaptiveTextStyle.Base
                                    },
                                    new AdaptiveText()
                                    {
                                        Text =feed.Desc,
                                        HintStyle=AdaptiveTextStyle.Body,
                                        HintWrap=true,
                                        HintMaxLines=4
                                    },
                                    new AdaptiveText()
                                    {
                                        Text=feed.Date,
                                        HintStyle =AdaptiveTextStyle.CaptionSubtle,
                                        HintWrap=true
                                    },
                                }
                            }
                        }
                    }
                };
                var notification = new TileNotification(content.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
            catch { return; }

        }


        private class Feeds
        {
            public bool is_history { get; set; }
            public int counts { get; set; }
            public Feedlist[] feedList { get; set; }
            public string message { get; set; }
            public bool more { get; set; }
            public string result { get; set; }
        }

        private class Feedlist
        {
            public string type { get; set; }
            public bool is_marked { get; set; }
            public Entry entry { get; set; }
        }

        private class Entry
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
            public bool delete { get; set; }
            public bool recommend { get; set; }
            public object is_self { get; set; }
            public bool rewardable { get; set; }
            public int rewards { get; set; }
            public bool wallpaper { get; set; }
            public int views { get; set; }
            public bool collected { get; set; }
            public int downloads { get; set; }
            public int shares { get; set; }
            public int collect_num { get; set; }
            public string content { get; set; }
            public bool update { get; set; }
            public Image[] images { get; set; }
            public Title_Image title_image { get; set; }
            public Tag[] tags { get; set; }
            public Topic[] topics { get; set; }
            public Comment_List[] comment_list { get; set; }
            public Comment_Best comment_best { get; set; }
            public User[] users { get; set; }
            public int ledger_status { get; set; }
            public string ledger_success_img_id { get; set; }
            public Site site { get; set; }
            public bool is_favorite { get; set; }
            public bool is_top { get; set; }
            public string data_type { get; set; }
            public string created_at { get; set; }
            public Site1[] sites { get; set; }
            public object attend_event { get; set; }
            public Music music { get; set; }
            public Recom_Type recom_type { get; set; }
            public string rqt_id { get; set; }
            public bool last_read { get; set; }
            public Equip equip { get; set; }
            public string app_url { get; set; }
            public int vid { get; set; }
            public string video_id { get; set; }
            public Author2 author { get; set; }
            public string cover { get; set; }
            public string raw_cover { get; set; }
            public string video_width { get; set; }
            public string video_height { get; set; }
            public string created { get; set; }
            public string share_url { get; set; }
            public string share_cover { get; set; }
            public string duration { get; set; }
            public bool is_recommend { get; set; }
            public string passed_time { get; set; }
            public bool is_ultra { get; set; }
            public object[] category { get; set; }
            public Cover_Detail cover_detail { get; set; }
            public bool liked { get; set; }
            public bool is_following { get; set; }
            public string video_model { get; set; }
        }

        private class Title_Image
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
            public int img_id { get; set; }
        }

        private class Comment_Best
        {
            public int note_id { get; set; }
            public Author author { get; set; }
            public string content { get; set; }
            public int sub_notes_count { get; set; }
            public bool liked { get; set; }
            public int likes { get; set; }
            public string created_at { get; set; }
            public Last_Replied last_replied { get; set; }
            public object[] images { get; set; }
            public string sort_weight { get; set; }
            public string note_type { get; set; }
            public string author_id { get; set; }
            public bool author_liked { get; set; }
            public Sub_Notes[] sub_notes { get; set; }
        }

        private class Author
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
        }

        private class Last_Replied
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
            public int followers { get; set; }
            public string recommend_reason { get; set; }
        }

        private class Sub_Notes
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

        private class Author1
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

        private class Verification_List
        {
            public int verification_type { get; set; }
            public string verification_reason { get; set; }
        }

        private class Site
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
            public string recommend_reason { get; set; }
            public bool verified { get; set; }
            public int verifications { get; set; }
            public Verification_List1[] verification_list { get; set; }
            public Ac_Camera ac_camera { get; set; }
            public Owner_Tag[] owner_tag { get; set; }
            public bool is_following { get; set; }
            public bool is_follower { get; set; }
        }

        private class Appearance
        {
            public string color { get; set; }
            public string image { get; set; }
        }

        private class Ac_Camera
        {
            public int obtained_num { get; set; }
            public Adorned adorned { get; set; }
        }

        private class Adorned
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

        private class Verification_List1
        {
            public int verification_type { get; set; }
            public string verification_reason { get; set; }
        }

        private class Owner_Tag
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

        private class Music
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

        private class Recom_Type
        {
            public string type { get; set; }
            public string reason { get; set; }
            public string recall_type { get; set; }
            public object[] entry { get; set; }
        }

        private class Equip
        {
            public string display_name { get; set; }
            public string equip_id { get; set; }
        }

        private class Author2
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
            public Verification_List2[] verification_list { get; set; }
            public Wallpaper wallpaper { get; set; }
            public Live_Wallpaper live_wallpaper { get; set; }
            public bool is_following { get; set; }
        }

        private class Wallpaper
        {
            public bool wallpaper_following { get; set; }
            public bool wallpaper_reward { get; set; }
        }

        private class Live_Wallpaper
        {
            public bool live_wallpaper_following { get; set; }
            public bool live_wallpaper_reward { get; set; }
        }

        private class Verification_List2
        {
            public int verification_type { get; set; }
            public string verification_reason { get; set; }
        }

        private class Cover_Detail
        {
            public string img_id { get; set; }
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        private class Image
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

        private class Tag
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

        private class Topic
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
            public string cover_url { get; set; }
        }

        private class Comment_List
        {
            public int note_id { get; set; }
            public Author3 author { get; set; }
            public string content { get; set; }
            public int sub_notes_count { get; set; }
            public bool liked { get; set; }
            public int likes { get; set; }
            public string created_at { get; set; }
            public Last_Replied1 last_replied { get; set; }
            public object[] images { get; set; }
            public string sort_weight { get; set; }
            public string note_type { get; set; }
            public string author_id { get; set; }
            public bool author_liked { get; set; }
            public Sub_Notes1[] sub_notes { get; set; }
        }

        private class Author3
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
        }

        private class Last_Replied1
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
            public int followers { get; set; }
            public string recommend_reason { get; set; }
        }

        private class Sub_Notes1
        {
            public int note_id { get; set; }
            public Author4 author { get; set; }
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

        private class Author4
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
            public Verification_List3[] verification_list { get; set; }
        }

        private class Verification_List3
        {
            public int verification_type { get; set; }
            public string verification_reason { get; set; }
        }

        private class User
        {
            public string site_id { get; set; }
            public string name { get; set; }
            public string icon { get; set; }
        }

        private class Site1
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
            public int followers { get; set; }
            public int members { get; set; }
            public int group_posts { get; set; }
            public string recommend_reason { get; set; }
            public bool verified { get; set; }
            public int verifications { get; set; }
            public Verification_List4[] verification_list { get; set; }
            public Ac_Camera1 ac_camera { get; set; }
            public Owner_Tag1[] owner_tag { get; set; }
            public Image1[] images { get; set; }
            public bool is_following { get; set; }
            public bool is_follower { get; set; }
        }

        private class Appearance1
        {
            public string color { get; set; }
            public string image { get; set; }
        }

        private class Ac_Camera1
        {
            public int obtained_num { get; set; }
            public Adorned1 adorned { get; set; }
        }

        private class Adorned1
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

        private class Verification_List4
        {
            public int verification_type { get; set; }
            public string verification_reason { get; set; }
        }

        private class Owner_Tag1
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

        private class Image1
        {
            public int img_id { get; set; }
            public string img_id_str { get; set; }
            public int user_id { get; set; }
            public string title { get; set; }
            public string excerpt { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int is_authorized_tc { get; set; }
        }


        private class FeedVM
        {
            public string Name { get; set; }
            public string Date { get; set; }
            public string Cover { get; set; }
            public string Desc { get; set; }

            public FeedVM(Feedlist feedlist)
            {
                this.Cover = "https://photo.tuchong.com/" + feedlist.entry.images[0].user_id + "/f/" + feedlist.entry.images[0].img_id + ".jpg";
                this.Name = "© " + feedlist.entry.site.name;
                this.Date = "发布于 " + feedlist.entry.published_at.Substring(0, 10).Trim();
                string desc = feedlist.entry.content.Replace("<br />", "");
                if (desc.Length <= 0)
                {
                    desc = "作者未添加描述";
                }
                this.Desc = desc;
            }
        }
    }
}