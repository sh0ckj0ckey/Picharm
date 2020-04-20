using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace 图虫.Helpers
{
    public static class CrawlerHelper
    {
        /// <summary>
        /// 获取指定网址的HTML
        /// </summary>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        public async static Task<string> GetHtml(string webUrl)
        {
            string html;
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            };
            HttpClient http = new HttpClient(httpClientHandler);
            try
            {
                var response = await http.GetAsync(new Uri(webUrl));
                html = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return "";
            }
            return html;
        }

        ///// <summary>
        ///// 获取用户的头像和名字
        ///// </summary>
        ///// <param name="userLink">用户主页链接</param>
        ///// <returns> result[0]: 头像路径 </returns>
        ///// <returns> result[1]: 用户名字 </returns>
        ///// <returns> result[2]: 用户粉丝和关注数量 </returns>
        ///// <returns> result[3]: 用户简介 </returns>
        ///// <returns> result[4]: 用户位置 </returns>
        //public async static Task<string[]> GetAvatar(string userLink)
        //{
        //    string[] result = new string[6];

        //    string html = await GetHtml(userLink);

        //    try
        //    {
        //        Match htmlPart = Regex.Match(html, "personal-logo-link([\\s\\S]*?)window.site");
        //        html = htmlPart.Groups[1].Value.Replace(" ", "");

        //        if (html.Contains("vip-rightvip-yellow"))
        //        {
        //            Match matchPhoto = Regex.Match(html, "<imgsrc=\\\"([\\s\\S]*?)\\\">");
        //            Match matchName = Regex.Match(html, "divclass=\\\"title\\\">([\\s\\S]*?)</div>");
        //            Match matchForward = Regex.Match(html, "following/\\\">([\\s\\S]*?)</a>");
        //            Match matchFans = Regex.Match(html, "followers/\\\">([\\s\\S]*?)</a>");
        //            Match matchBio = Regex.Match(html, "intro-text\\\">([\\s\\S]*?)</p>");
        //            Match matchLocation = Regex.Match(html, "info-list\\\">[\\s\\S]*?liclass=\\\"info-item\\\">([\\s\\S]*?)</li>");
        //            Match matchTitle = Regex.Match(html, "<pclass=\\\"info-desc\\\"title=\\\"([\\s\\S]*?)\\\">");
        //            result[0] = matchPhoto.Groups[1].Value;
        //            result[1] = matchName.Groups[1].Value;
        //            result[2] = matchForward.Groups[1].Value + "  " + matchFans.Groups[1].Value;
        //            result[3] = matchBio.Groups[1].Value;
        //            result[4] = matchLocation.Groups[1].Value;
        //            result[5] = matchTitle.Groups[1].Value;
        //        }
        //        else
        //        {
        //            Match matchPhoto = Regex.Match(html, "<imgsrc=\\\"([\\s\\S]*?)\\\">");
        //            Match matchName = Regex.Match(html, "info-name\\\">([\\s\\S]*?)</span>");
        //            Match matchForward = Regex.Match(html, "following/\\\">([\\s\\S]*?)</a>");
        //            Match matchFans = Regex.Match(html, "followers/\\\">([\\s\\S]*?)</a>");
        //            Match matchBio = Regex.Match(html, "intro-text\\\">([\\s\\S]*?)</p>");
        //            Match matchLocation = Regex.Match(html, "<spanclass=\\\"info-name\\\">[\\s\\S]*?slash-item\\\">([\\s\\S]*?)</li>");
        //            result[0] = matchPhoto.Groups[1].Value;
        //            result[1] = matchName.Groups[1].Value;
        //            result[2] = matchForward.Groups[1].Value + "  " + matchFans.Groups[1].Value;
        //            result[3] = matchBio.Groups[1].Value;
        //            result[4] = matchLocation.Groups[1].Value;
        //            result[5] = null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //    return result;
        //}
    }
}
