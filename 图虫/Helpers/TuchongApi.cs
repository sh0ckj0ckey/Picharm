using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using 图虫.ViewModels;

namespace 图虫
{
    public static class TuchongApi
    {
        /// <summary>
        /// 获取“推荐”内容
        /// </summary>
        /// <param name="para">API参数</param>
        /// <returns></returns>
        public static async Task<Models.Feeds.Feed> GetFeed(string para)
        {
            try
            {
                string url = "https://api.tuchong.com/2/feed-app?" + para;
                using (HttpClient http = new HttpClient())
                {
                    Models.Feeds.Feed feed;
                    try
                    {
                        try
                        {
                            http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                            http.DefaultRequestHeaders.Add("platform", "ios");
                        }
                        catch { }
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                        };
                        feed = JsonConvert.DeserializeObject<Models.Feeds.Feed>(jsonMessage, jss);
                    }
                    catch (Exception e)
                    {
                        return new Models.Feeds.Feed
                        {
                            message = "获取或者解析数据失败，可能是服务器问题，请尝试刷新\n " + e.Message
                        };
                    }
                    return feed;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取指定图集的评论
        /// </summary>
        /// <param name="para">API参数</param>
        /// <returns></returns>
        public static async Task<Models.Comments.Comments> GetComments(string para)
        {
            try
            {
                string url = "https://tuchong.com/rest/2/posts/" + para;
                using (HttpClient http = new HttpClient())
                {
                    Models.Comments.Comments comments;
                    try
                    {
                        // 已经登录的话
                        if (LoginHelper.UserCookie.Length > 4)
                        {
                            http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                        }

                        // 否则就不加 Cookie 直接获取
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        comments = JsonConvert.DeserializeObject<Models.Comments.Comments>(jsonMessage, jss);
                    }
                    catch
                    {
                        comments = new Models.Comments.Comments();
                        return comments;
                    }
                    return comments;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取“发现”，包含当前可用的“分类”和“活动”
        /// </summary>
        /// <returns></returns>
        public static async Task<Models.Discover.Discover> GetDiscover()
        {
            try
            {
                string url = "https://api.tuchong.com/discover-app";
                using (HttpClient http = new HttpClient())
                {
                    Models.Discover.Discover discover;
                    try
                    {
                        // 已经登录的话
                        if (LoginHelper.UserCookie.Length > 4)
                        {
                            http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                        }

                        // 否则就不加 Cookie 直接获取
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        discover = JsonConvert.DeserializeObject<Models.Discover.Discover>(jsonMessage, jss);
                    }
                    catch
                    {
                        return null;
                    }
                    return discover;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 根据参数获取指定分类的内容
        /// </summary>
        /// <param name="para">API参数</param>
        /// <returns></returns>
        public static async Task<Models.CategoryItem.CategoryItem> GetCategoryItem(string para)
        {
            try
            {
                string url = "https://api.tuchong.com/discover/" + para;
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                    Models.CategoryItem.CategoryItem categoryItem;
                    try
                    {
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        categoryItem = JsonConvert.DeserializeObject<Models.CategoryItem.CategoryItem>(jsonMessage, jss);
                    }
                    catch
                    {
                        return null;
                    }
                    return categoryItem;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 根据参数获取作者的作品集
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.Photograph.Photograph> GetPhotograph(string para, string id)
        {
            try
            {
                string url = "https://tuchong.com/rest/2/sites/" + id + "/posts?" + para;
                using (HttpClient http = new HttpClient())
                {
                    try
                    {
                        if (LoginHelper.UserCookie.Length > 4)
                        {
                            http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                        }
                    }
                    catch
                    {
                        try
                        {
                            http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                            http.DefaultRequestHeaders.Add("platform", "ios");
                        }
                        catch { }
                    }
                    Models.Photograph.Photograph photographs;
                    try
                    {
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        photographs = JsonConvert.DeserializeObject<Models.Photograph.Photograph>(jsonMessage, jss);
                    }
                    catch
                    {
                        photographs = null;
                    }
                    return photographs;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 根据图片ID获取其exif信息
        /// </summary>
        /// <param name="imgID">图片ID</param>
        /// <returns></returns>
        public static async Task<ObservableCollection<ImageExifViewModel>> GetImageExif(string imgID)
        {
            using (HttpClient http = new HttpClient())
            {
                ObservableCollection<ImageExifViewModel> result = new ObservableCollection<ImageExifViewModel>();
                try
                {
                    var response = await http.GetAsync(new Uri("https://api.tuchong.com/images/" + imgID + "/exif"));
                    var jsonMessage = await response.Content.ReadAsStringAsync();
                    jsonMessage = Unicode2String(jsonMessage).Replace("\\", "");

                    MatchCollection matchCollection = Regex.Matches(jsonMessage, "\\\"desc\\\":\\\"([\\s\\S]*?)\\\",\\\"content\\\":\\\"([\\s\\S]*?)\\\"");
                    ImageExifViewModel imageExifViewModel = null;
                    foreach (Match item in matchCollection)
                    {
                        imageExifViewModel = new ImageExifViewModel
                        {
                            Desc = item.Groups[1].Value,
                            Content = item.Groups[2].Value
                        };
                        result.Add(imageExifViewModel);
                    }
                }
                catch
                { }
                return result;
            }
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="para">API参数</param>
        /// <returns></returns>
        public static async Task<bool?> PutFavorite(string para)
        {
            try
            {
                string url = "https://tuchong.com/rest/users/self/favorites/" + para;
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                    try
                    {
                        var response = await http.PutAsync(new Uri(url), new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"));
                        var jsonMessage = await response.Content.ReadAsStringAsync();
                        if (jsonMessage.Contains("SUCCESS"))
                        {
                            // 操作成功
                            return true;
                        }
                        else if (jsonMessage.Contains("\"result\":\"ERROR\",\"code\":1"))
                        {
                            // 没有登录
                            return null;
                        }
                        else
                        {
                            // 其他原因失败
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<bool?> DeleteFavorite(string para)
        {
            try
            {
                string url = "https://tuchong.com/rest/users/self/favorites/" + para;
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                    try
                    {
                        var response = await http.DeleteAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();
                        if (jsonMessage.Contains("SUCCESS"))
                        {
                            // 操作成功
                            return true;
                        }
                        else if (jsonMessage.Contains("\"result\":\"ERROR\",\"code\":1"))
                        {
                            // 没有登录
                            return null;
                        }
                        else
                        {
                            // 其他原因失败
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="para">API参数</param>
        /// <param name="comment">评论内容</param>
        /// <returns></returns>
        public static async Task<string> PostComment(string postid, string comment)
        {
            try
            {
                string url = "https://api.tuchong.com/3/posts/" + postid + "/comments";
                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("content", comment),
                    new KeyValuePair<string, string>("position", "page_comment_list")
                };
                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                    http.DefaultRequestHeaders.Add("platform", "ios");
                    try
                    {
                        var response = await http.SendAsync(req);
                        var jsonMessage = await response.Content.ReadAsStringAsync();
                        Match match = Regex.Match(jsonMessage, "\\\"result\\\":\\\"([\\s\\S]*?)\\\"");
                        string result = match.Groups[1].Value;
                        if (result.ToUpper() == "SUCCESS")
                        {
                            // 操作成功
                            return "评论成功";
                        }
                        else
                        {
                            string msg;
                            try
                            {
                                Match messageMatch = Regex.Match(jsonMessage, "\\\"message\\\":\\\"([\\s\\S]*?)\\\"");
                                msg = messageMatch.Groups[1].Value;
                            }
                            catch { msg = "未知错误"; }
                            return "评论失败：" + Unicode2String(msg);
                        }
                    }
                    catch (Exception e)
                    {
                        return "评论发生异常：" + e.Message;
                    }
                }
            }
            catch (Exception e)
            {
                return "评论发生异常：" + e.Message;
            }
        }

        /// <summary>
        /// 获取搜索用户结果
        /// </summary>
        /// <param name="query"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.SitesSearchResult.SitesSearchResult> GetSitesSearchResult(string query, string para = "&count=20&page=1")
        {
            try
            {
                string url_sites = "https://tuchong.com/rest/3/search/sites?query=" + query + para;
                using (HttpClient http = new HttpClient())
                {
                    Models.SitesSearchResult.SitesSearchResult searchResult_sites = null;
                    try
                    {
                        var response_sites = await http.GetAsync(new Uri(url_sites));
                        var jsonMessage_sites = await response_sites.Content.ReadAsStringAsync();
                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        searchResult_sites = JsonConvert.DeserializeObject<Models.SitesSearchResult.SitesSearchResult>(jsonMessage_sites, jss);
                    }
                    catch
                    {
                        return null;
                    }
                    return searchResult_sites;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取搜索标签结果
        /// </summary>
        /// <param name="query"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.TagsSearchResult.TagsSearchResult> GetTagsSearchResult(string query, string para = "&count=20&page=1")
        {
            try
            {
                string url_tags = "https://tuchong.com/rest/3/search/tags?query=" + query + para;
                using (HttpClient http = new HttpClient())
                {
                    Models.TagsSearchResult.TagsSearchResult searchResult_tags = null;
                    try
                    {
                        var response_tags = await http.GetAsync(new Uri(url_tags));
                        var jsonMessage_tags = await response_tags.Content.ReadAsStringAsync();
                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        searchResult_tags = JsonConvert.DeserializeObject<Models.TagsSearchResult.TagsSearchResult>(jsonMessage_tags, jss);
                    }
                    catch
                    {
                        return null;
                    }
                    return searchResult_tags;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取搜索作品结果
        /// </summary>
        /// <param name="query"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.PostsSearchResult.PostsSearchResult> GetPostsSearchResult(string query, string para = "&count=20&page=1")
        {
            try
            {
                string url_posts = "https://tuchong.com/rest/3/search/posts?query=" + query + para;
                using (HttpClient http = new HttpClient())
                {
                    Models.PostsSearchResult.PostsSearchResult searchResult_posts = null;
                    try
                    {
                        var response_posts = await http.GetAsync(new Uri(url_posts));
                        var jsonMessage_posts = await response_posts.Content.ReadAsStringAsync();
                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        searchResult_posts = JsonConvert.DeserializeObject<Models.PostsSearchResult.PostsSearchResult>(jsonMessage_posts, jss);
                    }
                    catch
                    {
                        return null;
                    }
                    return searchResult_posts;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取摄影师的个人资料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Models.Photographer.PhotographerInfo> GetPhotographerInfo(string id)
        {
            try
            {
                string url = "https://api.tuchong.com/2/sites/" + id;
                using (HttpClient http = new HttpClient())
                {
                    Models.Photographer.PhotographerInfo photographerInfo;
                    try
                    {
                        if (LoginHelper.Token.Length > 0)
                        {
                            http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                        }
                        http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        photographerInfo = JsonConvert.DeserializeObject<Models.Photographer.PhotographerInfo>(jsonMessage, jss);
                    }
                    catch
                    {
                        return null;
                    }
                    return photographerInfo;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取我的关注列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.MyFollow.MyFollow> GetMyFollow(string id, string para = "count=20&page=1")
        {
            try
            {
                string url = "https://api.tuchong.com/users/" + id + "/following?" + para;
                using (HttpClient http = new HttpClient())
                {
                    if (LoginHelper.Token.Length > 0)
                    {
                        http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                    }
                    http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                    //http.DefaultRequestHeaders.Add("platform", "ios");
                    Models.MyFollow.MyFollow follow;
                    try
                    {
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        follow = JsonConvert.DeserializeObject<Models.MyFollow.MyFollow>(jsonMessage, jss);
                        return follow;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取我的粉丝列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.MyFans.MyFans> GetMyFans(string id, string para = "count=20&page=1")
        {
            try
            {
                string url = "https://api.tuchong.com/sites/" + id + "/followers?" + para;
                using (HttpClient http = new HttpClient())
                {
                    if (LoginHelper.Token.Length > 0)
                    {
                        http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                    }
                    http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                    //http.DefaultRequestHeaders.Add("platform", "ios");
                    Models.MyFans.MyFans follow;
                    try
                    {
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        follow = JsonConvert.DeserializeObject<Models.MyFans.MyFans>(jsonMessage, jss);
                        return follow;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取我的喜欢列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<Models.MyLike.MyLike> GetMyLike(string id, string para = "count=20")
        {
            try
            {
                string url = "https://api.tuchong.com/sites/" + id + "/favorites?" + para;
                using (HttpClient http = new HttpClient())
                {
                    if (LoginHelper.Token.Length > 0)
                    {
                        http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                    }
                    http.DefaultRequestHeaders.Add("Cookie", LoginHelper.UserCookie);
                    //http.DefaultRequestHeaders.Add("platform", "ios");
                    Models.MyLike.MyLike like;
                    try
                    {
                        var response = await http.GetAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        like = JsonConvert.DeserializeObject<Models.MyLike.MyLike>(jsonMessage, jss);
                        return like;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<PutFollowResp> PutFollow(string id)
        {
            try
            {
                string url = "https://api.tuchong.com/users/self/following/" + id;

                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("position", "tab_home_recommend"),
                    new KeyValuePair<string, string>("referer", "page_me")
                };
                var req = new HttpRequestMessage(HttpMethod.Put, url) { Content = new FormUrlEncodedContent(nvc) };

                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                    http.DefaultRequestHeaders.Add("platform", "ios");
                    try
                    {
                        var response = await http.SendAsync(req);
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        PutFollowResp resp = JsonConvert.DeserializeObject<PutFollowResp>(jsonMessage, jss);
                        return resp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<DeleteFollowResp> DeleteFollow(string id)
        {
            try
            {
                string url = "https://api.tuchong.com/users/self/following/" + id + "?position=page_user_attention&referer=page_me";
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("token", LoginHelper.Token);
                    http.DefaultRequestHeaders.Add("platform", "ios");
                    try
                    {
                        var response = await http.DeleteAsync(new Uri(url));
                        var jsonMessage = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings jss = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        DeleteFollowResp resp = JsonConvert.DeserializeObject<DeleteFollowResp>(jsonMessage, jss);
                        return resp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch { return null; }
        }


        /// <summary>
        /// 将Unicode字符转换为可读的汉字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static string Unicode2String(string source)
        {
            try
            {
                return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
            }
            catch { return ""; }
        }

    }

    public class PutFollowResp
    {
        public string message { get; set; }
        public string hint { get; set; }
        public bool showPointToast { get; set; }
        public int point { get; set; }
        public string result { get; set; }
    }

    public class DeleteFollowResp
    {
        public string message { get; set; }
        public string result { get; set; }
    }
}