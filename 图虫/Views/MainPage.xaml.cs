using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Text.RegularExpressions;
using 图虫.ViewModels;
using 图虫.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace 图虫
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public IncrementalLoadingCollection FeedsObservableCollection = new IncrementalLoadingCollection();
        public static ApplicationDataContainer SettingContainer = ApplicationData.Current.LocalSettings;

        public ObservableCollection<Models.Discover.Category> TagsObservableCollection = new ObservableCollection<Models.Discover.Category>();
        public ObservableCollection<Models.Discover.Banner> BannersObservableCollection = new ObservableCollection<Models.Discover.Banner>();
        public ObservableCollection<HoteventViewModel> HoteventObservableCollection = new ObservableCollection<HoteventViewModel>();
        public ObservableCollection<FeedViewModel> CategoryItemsObservableCollection = new ObservableCollection<FeedViewModel>();

        public ObservableCollection<string> QuickCommentObservableCollection = new ObservableCollection<string>();

        /// <summary>
        /// 当前分类下图片的页数，第一页是refresh，单独写，所以这个就是从2开始
        /// </summary>
        public int CategoryPageIndex = 2;

        /// <summary>
        /// 用来控制轮播图的计时器
        /// </summary>
        public DispatcherTimer timer = null;

        /// <summary>
        /// 当用户点击了分享或者评论后给这个变量赋值，传递给分享界面
        /// </summary>
        FeedViewModel ReadingFeed = null;

        /// <summary>
        /// 代表标签的索引，默认是-2，即 “最新”
        /// </summary>
        public int TagIndex = -2;

        public MainPage()
        {
            this.InitializeComponent();

            // 设置标题栏样式
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(18, 0, 0, 0);
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(26, 0, 0, 0);
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonPressedForegroundColor = Colors.Black;
            titleBar.ButtonInactiveForegroundColor = Colors.Gray;

            // 设置亚克力样式
            CheckAcrylic();

            // 检查网络
            if (NetworkHelper.CheckNetwork() == false)
            {
                NoteTextBlock.Text = "无法访问互联网";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
            }

            // 检测首次启动
            CheckFirstboot();

            // 初始化轮播图计时器，但不会直接启用
            SetFlipView();

            // 获取 “发现” 的 “活动” 内容
            GetDisCover();

            // 导航前往 BlankPage 会自动登录
            InfoFrame.Navigate(typeof(BlankPage));

            // 订阅事件，当 FeedListView 滑动到底部
            Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
        }

        /// <summary>
        /// 获取 “发现”
        /// </summary>
        public async void GetDisCover()
        {
            Models.Discover.Discover discover = await ApiHelper.GetDiscover();
            if (discover == null)
            {
                NoteTextBlock.Text = "没能获取 \"分类\" 和 \"发现\"";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                return;
            }

            foreach (var item in discover.categories)
            {
                TagsObservableCollection.Add(item);
            }

            foreach (var item in discover.banners)
            {
                BannersObservableCollection.Add(item);
            }

            foreach (var item in discover.hotEvents)
            {
                HoteventObservableCollection.Add(new HoteventViewModel(item));
            }
        }

        /// <summary>
        /// 初始化定时轮播 FlipView
        /// </summary>
        public void SetFlipView()
        {
            int change = 1;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2.3)
            };
            timer.Tick += (o, a) =>
            {
                int newIndex = BannerFlipView.SelectedIndex + change;
                if (newIndex >= BannerFlipView.Items.Count || newIndex < 0)
                {
                    change *= -1;
                }
                BannerFlipView.SelectedIndex += change;
            };
        }

        /// 获取 “推荐” 在IncrementalLoadingCollection中实现

        /// <summary>
        /// 开关亚克力效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcrylicToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (AcrylicToggleSwitch.IsOn == true)
            {
                MainPageAcrylicBrush.AlwaysUseFallback = false;
                SettingContainer.Values["acrylic"] = "on";
            }
            else
            {
                MainPageAcrylicBrush.AlwaysUseFallback = true;
                SettingContainer.Values["acrylic"] = "off";
            }
        }

        /// <summary>
        /// 启动应用时检查设置，自动切换是否开启亚克力
        /// </summary>
        public void CheckAcrylic()
        {
            if (SettingContainer.Values["acrylic"] == null || SettingContainer.Values["acrylic"].ToString() == "off")
            {
                MainPageAcrylicBrush.AlwaysUseFallback = true;
                AcrylicToggleSwitch.IsOn = false;
            }
            else
            {
                MainPageAcrylicBrush.AlwaysUseFallback = false;
                AcrylicToggleSwitch.IsOn = true;
            }
        }

        /// <summary>
        /// 启动应用时检查设置，是否首次启动，显示更新日志
        /// </summary>
        public void CheckFirstboot()
        {
            if (SettingContainer.Values["firstboot"] == null || SettingContainer.Values["firstboot"].ToString() == "true")
            {
                UpdatelogStackPanel.Visibility = Visibility.Visible;
                SettingContainer.Values["firstboot"] = "false";
            }
            else
            {
                UpdatelogStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 显示程序异常对话框
        /// </summary>
        public static async void ShowDialog(string content)
        {
            var dialog = new ContentDialog()
            {
                Title = ":(",
                Content = content,
                PrimaryButtonText = "好的",
                FullSizeDesired = false
            };

            dialog.PrimaryButtonClick += (_s, _e) => { dialog.Hide(); };
            try
            {
                await dialog.ShowAsync();
            }
            catch { }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// 返回顶部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToTopButton_Click(object sender, RoutedEventArgs e)
        {
            FeedListView.SelectedIndex = 0;
            FeedListView.UpdateLayout();
            FeedListView.ScrollIntoView(FeedListView.SelectedItem);
        }

        /// <summary>
        /// 查看当前图集的全部图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = sender as AppBarButton;
            FeedViewModel oneFeed = (FeedViewModel)btn.DataContext;
            oneFeed.ShouldGoBack = false;
            InfoFrame.Navigate(typeof(MorePicturesPage), oneFeed);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            FeedViewModel oneFeed = (FeedViewModel)btn.DataContext;
            oneFeed.ShouldGoBack = false;
            InfoFrame.Navigate(typeof(MorePicturesPage), oneFeed);
        }


        #region 显示和消除 Toast 提示的动画
        private void DoubleAnimation_Completed(object sender, object e)
        {
            NoteNext.Begin();
        }

        private void DoubleAnimation_Completed_1(object sender, object e)
        {
            NoteLast.Begin();
        }

        private void DoubleAnimation_Completed_2(object sender, object e)
        {
            NoteGrid.Visibility = Visibility.Collapsed;
        }
        #endregion 

        /// <summary>
        /// 点击轮播图的活动，将前往浏览器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hlb = sender as HyperlinkButton;
            Models.Discover.Banner oneFeed = (Models.Discover.Banner)hlb.DataContext;
            Match eventID = Regex.Match(oneFeed.url, "id=([\\s\\S]*)");
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://tuchong.com/events/" + eventID.Groups[1].Value + "/"));
        }

        /// <summary>
        /// 点击列表中的活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EventListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (EventListView.ContainerFromItem(e.ClickedItem) is ListViewItem container)
            {
                HoteventViewModel hoteventViewModel = container.Content as HoteventViewModel;
                await Windows.System.Launcher.LaunchUriAsync(new Uri(hoteventViewModel.Url));
            }
        }

        /// <summary>
        /// 选择了一个分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryGridView.SelectedIndex >= 0)
            {
                CategoryPageIndex = 2;
                TagIndex = TagsObservableCollection[CategoryGridView.SelectedIndex].tag_id;
                CategoryLoadingStackPanel.Visibility = Visibility.Visible;
                CategoryLoadingProgressRing.IsActive = true;
                InitialCategoryItem(TagIndex);
            }
        }


        /// <summary>
        /// 根据ID获取相关分类的列表，初始化即获取第一页内容
        /// </summary>
        /// <param name="id"></param>
        public async void InitialCategoryItem(int id)
        {
            string para = id + "/category?page=1&type=refresh";
            Models.CategoryItem.CategoryItem categoryItem = await ApiHelper.GetCategoryItem(para);
            if (categoryItem == null)
            {
                NoteTextBlock.Text = "没能获取 \"分类\" 的数据";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                return;
            }
            CategoryItemsObservableCollection.Clear();
            CategoryItemsListView.ItemsSource = null;
            foreach (var item in categoryItem.post_list)
            {
                CategoryItemsObservableCollection.Add(new FeedViewModel(item));
            }
            CategoryItemsListView.ItemsSource = CategoryItemsObservableCollection;
            CategoryLoadingStackPanel.Visibility = Visibility.Collapsed;
            CategoryLoadingProgressRing.IsActive = false;
        }

        /// <summary>
        /// 分类中的列表加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (CategoryItemsObservableCollection.Count < 1)
            {
                NoteTextBlock.Text = "没能获取 \"分类\" 的数据，请尝试刷新";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                return;
            }
            string para = TagIndex + "/category?post_id=" + CategoryItemsObservableCollection.Last().PostID + "&page=" + CategoryPageIndex + "&type=loadmore";
            Models.CategoryItem.CategoryItem categoryItem = await ApiHelper.GetCategoryItem(para);
            if (categoryItem == null)
            {
                NoteTextBlock.Text = "没能获取更多 \"分类\" 的数据";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                return;
            }
            foreach (var item in categoryItem.post_list)
            {
                CategoryItemsObservableCollection.Add(new FeedViewModel(item));
            }
            CategoryPageIndex++;
        }

        /// <summary>
        /// 消除更新日志通知动画播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            IKnowTheUpdate.Begin();
        }

        /// <summary>
        /// 消除更新日志通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleAnimation_Completed_3(object sender, object e)
        {
            UpdatelogStackPanel.Visibility = Visibility.Collapsed;
            SettingContainer.Values["firstboot"] = "false";
        }

        /// <summary>
        /// 在分类栏目中点击了一项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FeedViewModel oneFeed = CategoryItemsObservableCollection[CategoryItemsListView.SelectedIndex];
                oneFeed.ShouldGoBack = false;
                InfoFrame.Navigate(typeof(MorePicturesPage), oneFeed);
            }
            catch { }
        }

        /// <summary>
        /// 切换“推荐” “分类” “发现”栏目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomePagePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomePagePivot.SelectedIndex == 0)
            {
                BackToTopButton.Visibility = Visibility.Visible;
                timer.Stop();
            }
            else if (HomePagePivot.SelectedIndex == 1)
            {
                BackToTopButton.Visibility = Visibility.Collapsed;
                timer.Stop();

                // 检测是否已经登录，控制 “分类” 板块是否可见
                if (LoginHelper.LoggedIn == true)
                {
                    LoadMoreDiscoverButton.Visibility = Visibility.Visible;
                    NoLoginGrid.Visibility = Visibility.Collapsed;
                    CategoryGridView.SelectedIndex = 0;
                }
                else
                {
                    NoLoginGrid.Visibility = Visibility.Visible;
                    LoadMoreDiscoverButton.Visibility = Visibility.Collapsed;
                }
            }
            else if (HomePagePivot.SelectedIndex == 2)
            {
                BackToTopButton.Visibility = Visibility.Collapsed;
                timer.Start();
            }
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = sender as AppBarButton;
            ReadingFeed = (FeedViewModel)btn.DataContext;
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        /// <summary>
        /// 请求数据以分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void MainPage_DataRequested(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            if (ReadingFeed != null)
            {
                string descText = "";
                if (ReadingFeed.Description.Length > 0 && ReadingFeed.Description.Length <= 28)
                {
                    descText = ReadingFeed.Description;
                }
                else if (ReadingFeed.Description.Length > 28)
                {
                    descText = ReadingFeed.Description.Substring(0, 28) + "...";
                }
                else
                {
                    descText = ReadingFeed.UserName + "的作品";
                }
                descText += "\n";
                descText += "https://tuchong.com/" + ReadingFeed.UserID.Substring(4) + "/" + ReadingFeed.PostID + "/";
                args.Request.Data.SetText(descText);
                args.Request.Data.SetWebLink(new Uri("https://tuchong.com/" + ReadingFeed.UserID.Substring(4) + "/" + ReadingFeed.PostID + "/"));
                args.Request.Data.Properties.Title = ReadingFeed.UserName + "的作品";
            }
            else
            {
                args.Request.FailWithDisplayText("好像没有东西可以分享");
            }
            ReadingFeed = null;
        }

        /// <summary>
        /// 打开评论窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommentAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginHelper.LoggedIn == false)
            {
                NoteTextBlock.Text = "需要先登录才可以评论哦";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                InfoFrame.Navigate(typeof(BeforeLoginPage));
                return;
            }

            AppBarButton btn = sender as AppBarButton;
            ReadingFeed = (FeedViewModel)btn.DataContext;

            CommentGrid.Visibility = Visibility.Visible;
            CommentGridPopIn.Begin();
            if (QuickCommentObservableCollection.Count <= 0)
            {
                QuickCommentObservableCollection.Add("不错");
                QuickCommentObservableCollection.Add("学习了");
                QuickCommentObservableCollection.Add("手动点赞");
                QuickCommentObservableCollection.Add("喜欢你的作品");
                QuickCommentObservableCollection.Add("喜欢你的拍摄手法");
            }
            QuickCommentListView.SelectedIndex = -1;
        }

        /// <summary>
        /// 关闭评论窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommentGridPopOut.Begin();
        }

        /// <summary>
        /// 关闭评论窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            CommentGridPopOut.Begin();
        }

        /// <summary>
        /// 关闭评论窗口动画结束后设置为不可见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommentGridPopOut_Completed(object sender, object e)
        {
            CommentGrid.Visibility = Visibility.Collapsed;
            CommentTextBox.Text = "";
            ReadingFeed = null;
        }

        /// <summary>
        /// 插入快速评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickCommentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuickCommentListView.SelectedIndex == -1)
            {
                return;
            }
            CommentTextBox.Text += QuickCommentObservableCollection[QuickCommentListView.SelectedIndex];
            QuickCommentListView.SelectedIndex = -1;
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string commentContent = CommentTextBox.Text;
            string sent = await ApiHelper.PostComment(ReadingFeed.PostID, commentContent);

            NoteTextBlock.Text = sent;
            NoteGrid.Visibility = Visibility.Visible;
            NoteFirst.Begin();

            CommentTextBox.Text = "";
            CommentGridPopOut.Begin();
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LikeAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginHelper.LoggedIn == false)
            {
                NoteTextBlock.Text = "需要先登录才可以点赞哦";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                InfoFrame.Navigate(typeof(BeforeLoginPage));
                return;
            }

            AppBarButton btn = sender as AppBarButton;
            FeedViewModel feedViewModel = (FeedViewModel)btn.DataContext;
            //FeedsObservableCollection[collectionIndex].IsFavarite = FeedsObservableCollection[collectionIndex].IsFavarite == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            feedViewModel.isFavarite = feedViewModel.isFavarite == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            feedViewModel.Like = feedViewModel.isFavarite == Visibility.Visible ? feedViewModel.Like + 1 : feedViewModel.Like - 1;
            bool? sent;
            if (feedViewModel.isFavarite == Visibility.Visible)
            {
                sent = await ApiHelper.PutFavorite(feedViewModel.PostID);
            }
            else
            {
                sent = await ApiHelper.DeleteFavorite(feedViewModel.PostID);
            }

            //如果点赞失败了就提示，并还原到点击之前的样子
            if (sent == null)
            {
                NoteTextBlock.Text = "登录失败，无法点赞，请重新登录";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                feedViewModel.isFavarite = feedViewModel.isFavarite == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                feedViewModel.Like = feedViewModel.isFavarite == Visibility.Visible ? feedViewModel.Like + 1 : feedViewModel.Like - 1;
            }
            else if (sent == false)
            {
                NoteTextBlock.Text = "点赞失败，请稍后重试~";
                NoteGrid.Visibility = Visibility.Visible;
                NoteFirst.Begin();
                feedViewModel.isFavarite = feedViewModel.isFavarite == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                feedViewModel.Like = feedViewModel.isFavarite == Visibility.Visible ? feedViewModel.Like + 1 : feedViewModel.Like - 1;
            }
        }

        /// <summary>
        /// 跳转至用户资料页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            FeedViewModel feedViewModel = (FeedViewModel)btn.DataContext;
            MsgBus.Instance.PhotographerID = feedViewModel.UserID;
            InfoFrame.Navigate(typeof(PhotographerPage), false);
        }

        private void CommentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CommentTextBox.Text.Length > 0)
            {
                CommentButton.IsEnabled = true;
            }
            else
            {
                CommentButton.IsEnabled = false;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Trim() != "")
            {
                InfoFrame.Navigate(typeof(SearchPage), SearchTextBox.Text);
            }
        }

        private void SearchTextBox_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (SearchTextBox.Text.Trim() != "")
            {
                e.Handled = true;
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    InfoFrame.Navigate(typeof(SearchPage), SearchTextBox.Text);
                }
            }
        }
    }
}
