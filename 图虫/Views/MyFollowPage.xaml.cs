using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 图虫.Helpers;
using 图虫.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyFollowPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private MyFollowViewModel _viewModel = new MyFollowViewModel();
        MyFollowViewModel ViewModel
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

        private string beforeTimestamp = "";
        private int page = 1;

        public MyFollowPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter.GetType().Equals(typeof(BitmapImage)))
                {
                    this.ViewModel.Avatar = (BitmapImage)e.Parameter;
                }
                if (MsgBus.Instance.ShouldRefreshMyFollowing)
                {
                    ViewModel.ResetData();
                    beforeTimestamp = "";
                    page = 1;
                    MsgBus.Instance.ShouldRefreshMyFollowing = false;
                    GetFollow();
                }
            }
            catch { }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                ViewModel.ResetData();
                this.Frame.GoBack();
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                AppBarButton bt = sender as AppBarButton;
                Models.MyFollow.Site site = (Models.MyFollow.Site)bt.DataContext;
                Helpers.MsgBus.Instance.PhotographerID = "ID: " + site.site_id;
                this.Frame.Navigate(typeof(PhotographerPage), true);
            }
            catch { }
        }

        /// <summary>
        /// 加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            GetFollow("before_timestamp=" + beforeTimestamp + "&count=20&page=" + page);
        }

        private async void GetFollow(string para = "count=20&page=1")
        {
            try
            {
                LoadingProgressBar.IsIndeterminate = true;
                LoadingProgressBar.Visibility = Visibility.Visible;

                var followList = await TuchongApi.GetMyFollow(LoginHelper.GetUserID(), para);
                if (followList != null && followList.result == "SUCCESS")
                {
                    this.ViewModel.HasMore = followList.more ? Visibility.Visible : Visibility.Collapsed;
                    this.ViewModel.End = followList.more ? Visibility.Collapsed : Visibility.Visible;
                    beforeTimestamp = followList.before_timestamp;
                    page++;
                    foreach (var item in followList.sites)
                    {
                        this.ViewModel.FollowsList.Add(item);
                    }
                }
                else
                {
                    FailedStackPanel.Visibility = Visibility.Visible;
                    MyFollowScrollViewer.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                FailedStackPanel.Visibility = Visibility.Visible;
                MyFollowScrollViewer.Visibility = Visibility.Collapsed;
            }
            finally
            {
                LoadingProgressBar.IsIndeterminate = false;
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.ResetData();
                beforeTimestamp = "";
                page = 1;
                GetFollow();
            }
            catch { }
        }
    }
}
