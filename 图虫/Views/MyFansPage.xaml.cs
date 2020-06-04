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
    public sealed partial class MyFansPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private MyFansViewModel _viewModel = new MyFansViewModel();
        MyFansViewModel ViewModel
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

        public MyFansPage()
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
                if (MsgBus.Instance.ShouldRefreshMyFans)
                {
                    ViewModel.ResetData();
                    beforeTimestamp = "";
                    page = 1;
                    MsgBus.Instance.ShouldRefreshMyFans = false;
                    GetFans();
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
                Models.MyFans.Site site = (Models.MyFans.Site)bt.DataContext;
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
            GetFans("before_timestamp=" + beforeTimestamp + "&count=20&page=" + page);
        }

        private async void GetFans(string para = "count=20&page=1")
        {
            try
            {
                LoadingProgressBar.IsIndeterminate = true;
                LoadingProgressBar.Visibility = Visibility.Visible;

                var fansList = await TuchongApi.GetMyFans(LoginHelper.GetUserID(), para);
                if (fansList != null && fansList.result == "SUCCESS")
                {
                    this.ViewModel.HasMore = fansList.more ? Visibility.Visible : Visibility.Collapsed;
                    this.ViewModel.End = fansList.more ? Visibility.Collapsed : Visibility.Visible;
                    beforeTimestamp = fansList.before_timestamp;
                    page++;
                    foreach (var item in fansList.sites)
                    {
                        this.ViewModel.FansList.Add(item);
                    }
                }
                else
                {
                    FailedStackPanel.Visibility = Visibility.Visible;
                    MyFansScrollViewer.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                FailedStackPanel.Visibility = Visibility.Visible;
                MyFansScrollViewer.Visibility = Visibility.Collapsed;
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
                GetFans();
            }
            catch { }
        }
    }
}
