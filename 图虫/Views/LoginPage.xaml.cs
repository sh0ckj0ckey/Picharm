using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 点击返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        /// <summary>
        /// 完成加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoginWebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByClassName('nav-login login-trigger')[0].click();" });
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByName('account')[0].value='" + LoginHelper.GetAccount() + "';" });
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByName('password')[0].value='" + LoginHelper.GetPassword() + "';" });
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByClassName('submit-btn')[0].click();" });
            }
            catch
            {
                //    var dialog = new ContentDialog()
                //    {
                //        Title = ":(",
                //        Content = "登录操作失败，请重试或者联系开发者",
                //        PrimaryButtonText = "好的",
                //        FullSizeDesired = false
                //    };

                //    dialog.PrimaryButtonClick += (_s, _e) => { };
                //    try
                //    {
                //        await dialog.ShowAsync();
                //    }
                //    catch { }
            }
        }

        /// <summary>
        /// 完成登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string cookie = await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.cookie;" });
                // System.Diagnostics.Debug.WriteLine(cookie);

                Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                Windows.Web.Http.HttpCookieCollection cookieCollection = filter.CookieManager.GetCookies(LoginWebView.Source);
                string iGotCookie = "";
                foreach (var cookiee in cookieCollection)
                {
                    iGotCookie += cookiee.Name + "=" + cookiee.Value + ";";
                    // System.Diagnostics.Debug.WriteLine(cookiee.Name + ": " + cookiee.Value + " HttpOnly: " + cookiee.HttpOnly);
                }
                LoginHelper.UserCookie = iGotCookie;

                string siteHTML = await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });

                Match userId = Regex.Match(siteHTML, "tuchong-avatar/ll_([\\s\\S]*?)_");
                LoginHelper.SetUserID(userId.Groups[1].Value);

                LoginHelper.LoggedIn = true;
            }
            catch
            {
                LoginHelper.LoggedIn = false;

                var dialog = new ContentDialog()
                {
                    Title = ":(",
                    Content = "登录失败，请重试一下或者检查网络重启应用",
                    PrimaryButtonText = "好的",
                    FullSizeDesired = false
                };

                dialog.PrimaryButtonClick += (_s, _e) => { dialog.Hide(); };
                try
                {
                    await dialog.ShowAsync();
                }
                catch { }

                return;
            }
            this.Frame.Navigate(typeof(BlankPage));
        }
    }
}
