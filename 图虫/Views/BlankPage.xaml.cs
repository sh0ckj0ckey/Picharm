using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 图虫.Helpers;
using 图虫.ViewModels;
using 图虫.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace 图虫
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// 用户对应的摄像师 VM
        /// </summary>
        private PhotographerViewModel _viewModel =
            new PhotographerViewModel
            {
                Name = "点击登录",
                Description = "登录后可以点赞、评论",
                Icon = new BitmapImage(new Uri("ms-appx:///Assets/Icons/unknown_user.png")),
                Location = "冷冽谷的伊鲁席尔"
            };
        PhotographerViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        //// 如果不是去搜索页面，则隐藏搜索框
        //private bool goingToSearch = false;

        public BlankPage()
        {
            this.InitializeComponent();

            if (LoginHelper.LoggedIn)
            {
                SetAvatar(LoginHelper.GetUserID());
            }
        }

        //protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        //{
        //    if (goingToSearch)
        //    {
        //        ConnectedAnimationService.GetForCurrentView()
        //            .PrepareToAnimate("goSearchAnimation", GoSearchAppBarButton);
        //    }
        //    else
        //    {
        //        GoSearchAppBarButton.Visibility = Visibility.Collapsed;
        //    }
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    goingToSearch = false;
        //    ConnectedAnimation animation =
        //        ConnectedAnimationService.GetForCurrentView().GetAnimation("backAnimation");
        //    if (animation != null)
        //    {
        //        animation.TryStart(GoSearchAppBarButton);
        //    }
        //}

        /// <summary>
        /// 设置页面显示当前登录用户的信息
        /// </summary>
        /// <param name="id"></param>
        public async void SetAvatar(string id)
        {
            try
            {
                //MyFollowAppBarButton.Visibility = Visibility.Visible;
                //MyLikeAppBarButton.Visibility = Visibility.Visible;
                //MyPageAppBarButton.Visibility = Visibility.Visible;
                FeaturesStackPanel.Visibility = Visibility.Visible;
                if (LoginHelper.HaveInfo == true)
                {
                    ViewModel = LoginHelper.UserInfo;
                }
                else
                {
                    var info = await TuchongApi.GetPhotographerInfo(id);
                    if (info != null)
                    {
                        ViewModel = new PhotographerViewModel(info);
                        ViewModel.Description = "关注" + ViewModel.Following + "  粉丝" + ViewModel.Followers;
                        ViewModel.Location = "";
                        LoginHelper.UserInfo = ViewModel;
                        LoginHelper.HaveInfo = true;
                    }
                }
                if (ViewModel.Name == "点击登录" && ViewModel.Name == "登录后可以点赞、评论" &&
                    ViewModel.Location == "冷冽谷的伊鲁席尔" && LoginHelper.LoggedIn)
                {
                    ViewModel.Name = "重新登录";
                    ViewModel.Description = "登录失败，点击重试";
                    ViewModel.Icon = new BitmapImage(new Uri("ms-appx:///Assets/Icons/unknown_user.png"));
                    ViewModel.Location = "冷冽谷的伊鲁席尔";
                    LoginHelper.LoggedIn = false;
                    FeaturesStackPanel.Visibility = Visibility.Collapsed;
                    //MyFollowAppBarButton.Visibility = Visibility.Collapsed;
                    //MyLikeAppBarButton.Visibility = Visibility.Collapsed;
                    //MyPageAppBarButton.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                ViewModel = new PhotographerViewModel();
                ViewModel.Name = "重新登录";
                ViewModel.Description = "登录失败，点击重试";
                ViewModel.Icon = new BitmapImage(new Uri("ms-appx:///Assets/Icons/unknown_user.png"));
                ViewModel.Location = "冷冽谷的伊鲁席尔";
                LoginHelper.LoggedIn = false;
                FeaturesStackPanel.Visibility = Visibility.Collapsed;
                //MyFollowAppBarButton.Visibility = Visibility.Collapsed;
                //MyLikeAppBarButton.Visibility = Visibility.Collapsed;
                //MyPageAppBarButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 点击登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BeforeLoginPage));
        }

        /// <summary>
        /// 加载完成登录页面后自动填充数据并提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoginWebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (LoginHelper.LoggedIn)
            {
                return;
            }

            // 首先检查是否已经有登录账号密码
            string acc = LoginHelper.GetAccount();
            string pwd = LoginHelper.GetPassword();
            if (acc == "" || pwd == "")
            {
                UserPhotoImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Icons/unknown_user.png"));
                ViewModel.Name = "点击登录";
                ViewModel.Description = "登录后可以点赞、评论";
                LoginHelper.LoggedIn = false;
                //MyFollowAppBarButton.Visibility = Visibility.Collapsed;
                //MyLikeAppBarButton.Visibility = Visibility.Collapsed;
                //MyPageAppBarButton.Visibility = Visibility.Collapsed;
                FeaturesStackPanel.Visibility = Visibility.Collapsed;
                return;
            }

            // 有账号密码的记录，将自动登录
            try
            {
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByClassName('nav-login login-trigger')[0].click();" });
            }
            catch { }
            try
            {
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByName('account')[0].value='" + LoginHelper.GetAccount() + "';" });
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByName('password')[0].value='" + LoginHelper.GetPassword() + "';" });
                await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByClassName('submit-btn')[0].click();" });
            }
            catch { }
            try
            {
                // 获取 Cookie
                Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                Windows.Web.Http.HttpCookieCollection cookieCollection = filter.CookieManager.GetCookies(LoginWebView.Source);
                string iGotCookie = "";
                foreach (var cookiee in cookieCollection)
                {
                    iGotCookie += cookiee.Name + "=" + cookiee.Value + ";";
                }
                LoginHelper.UserCookie = iGotCookie;

                // 获取用户的 ID，设置其头像和昵称
                var siteHTML = await LoginWebView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });

                Match userlink = Regex.Match(siteHTML, "tuchong-avatar/ll_([\\s\\S]*?)_");
                LoginHelper.SetUserID(userlink.Groups[1].Value);

                SetAvatar(LoginHelper.GetUserID());

                // 登录成功后设置为登入状态
                LoginHelper.LoggedIn = true;
            }
            catch
            {
                UserPhotoImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Icons/unknown_user.png"));
                ViewModel.Name = "点击登录";
                ViewModel.Description = "登录后可以点赞、评论";
                LoginHelper.LoggedIn = false;
                //MyFollowAppBarButton.Visibility = Visibility.Collapsed;
                //MyLikeAppBarButton.Visibility = Visibility.Collapsed;
                //MyPageAppBarButton.Visibility = Visibility.Collapsed;
                FeaturesStackPanel.Visibility = Visibility.Collapsed;
                return;
            }
        }

        /// <summary>
        /// 去搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void GoSearchAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    goingToSearch = true;
        //    Frame.Navigate(typeof(SearchPage), null, new SuppressNavigationTransitionInfo());
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        /// <summary>
        /// 我的主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginHelper.LoggedIn)
            {
                MsgBus.Instance.PhotographerID = "ID: " + LoginHelper.GetUserID();
                this.Frame.Navigate(typeof(PhotographerPage), true);
            }
        }

        /// <summary>
        /// 我的喜欢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (LoginHelper.LoggedIn)
            {
                MsgBus.Instance.PhotographerID = "ID: " + LoginHelper.GetUserID();
                MsgBus.Instance.ShouldRefreshMyLike = true;
                this.Frame.Navigate(typeof(MyLikePage), this.ViewModel.Icon, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }

        /// <summary>
        /// 我的关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            if (LoginHelper.LoggedIn)
            {
                MsgBus.Instance.PhotographerID = "ID: " + LoginHelper.GetUserID();
                MsgBus.Instance.ShouldRefreshMyFollowing = true;
                this.Frame.Navigate(typeof(MyFollowPage), this.ViewModel.Icon, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }

        /// <summary>
        /// 我的粉丝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            if (LoginHelper.LoggedIn)
            {
                MsgBus.Instance.PhotographerID = "ID: " + LoginHelper.GetUserID();
                MsgBus.Instance.ShouldRefreshMyFans = true;
                this.Frame.Navigate(typeof(MyFansPage), this.ViewModel.Icon, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }

    }
}
