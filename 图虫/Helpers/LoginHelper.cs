using System;
using System.Text.RegularExpressions;
using Windows.Storage;

namespace 图虫
{
    public static class LoginHelper
    {
        public static ApplicationDataContainer AccountContainer = ApplicationData.Current.LocalSettings;
        public static string UserCookie = "";

        /// <summary>
        /// Token，从 UserCookie 中取
        /// </summary>
        public static string Token
        {
            get
            {
                try
                {
                    Match match = Regex.Match(UserCookie, "token=([\\s\\S]*?);");
                    return match.Groups[1].Value;
                }
                catch
                {
                    return "";
                }
            }
        }
        public static bool LoggedIn { get; set; } = false;

        /// <summary>
        /// 缓存用户的资料
        /// </summary>
        public static ViewModels.PhotographerViewModel UserInfo = null;

        public static bool HaveInfo { get; set; } = false;

        /// <summary>
        /// 存取用户 ID
        /// </summary>
        /// <param name="id"></param>
        public static void SetUserID(string id) { AccountContainer.Values["userid"] = id; }
        public static string GetUserID()
        {
            if (AccountContainer.Values["userid"] == null || AccountContainer.Values["userid"].ToString() == "")
            {
                return "";
            }
            else
            {
                return AccountContainer.Values["userid"].ToString();
            }
        }

        /// <summary>
        /// 存取账号
        /// </summary>
        /// <param name="account"></param>
        public static void SetAccount(string account) { AccountContainer.Values["account"] = account; }
        public static string GetAccount()
        {
            if (AccountContainer.Values["account"] == null || AccountContainer.Values["account"].ToString() == "")
            {
                return "";
            }
            else
            {
                return AccountContainer.Values["account"].ToString();
            }
        }

        /// <summary>
        /// 存取密码
        /// </summary>
        /// <param name="password"></param>
        public static void SetPassword(string password) { AccountContainer.Values["password"] = password; }
        public static string GetPassword()
        {
            if (AccountContainer.Values["password"] == null || AccountContainer.Values["password"].ToString() == "")
            {
                return "";
            }
            else
            {
                return AccountContainer.Values["password"].ToString();
            }
        }

    }
}
