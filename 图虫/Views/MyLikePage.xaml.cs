
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 图虫.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyLikePage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private MyLikeViewModel _viewModel = new MyLikeViewModel();
        MyLikeViewModel ViewModel
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

        public MyLikePage()
        {
            this.InitializeComponent();
            GetLike();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType().Equals(typeof(BitmapImage)))
            {
                this.ViewModel.Avatar = (BitmapImage)e.Parameter;
            }

            base.OnNavigatedTo(e);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton bt = sender as AppBarButton;
            FeedViewModel para = (FeedViewModel)bt.DataContext;
            para.ShouldGoBack = true;
            this.Frame.Navigate(typeof(MorePicturesPage), para);
        }

        /// <summary>
        /// 加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            GetLike("before_timestamp=" + beforeTimestamp + "&count=20");
        }

        private async void GetLike(string para = "count=20")
        {
            try
            {
                var likeList = await ApiHelper.GetMyLike(LoginHelper.GetUserID(), para);
                if (likeList != null && likeList.result == "SUCCESS")
                {
                    this.ViewModel.HasMore = likeList.more ? Visibility.Visible : Visibility.Collapsed;
                    this.ViewModel.End = likeList.more ? Visibility.Collapsed : Visibility.Visible;
                    beforeTimestamp = likeList.before_timestamp;

                    foreach (var item in likeList.work_list)
                    {
                        if (item.type == "post")
                        {
                            this.ViewModel.LikeList.Add(new FeedViewModel(item.entry));
                        }
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
        }
    }
}
