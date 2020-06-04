
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MaterialLibs.Helpers;
using Windows.Networking.NetworkOperators;
using Windows.UI.Xaml.Media.Imaging;
using System;
using 图虫.ViewModels;
using 图虫.Helpers;
using System.Linq;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MorePicturesPage : Page
    {
        ObservableCollection<AsyncBitmapImage> allPictures = new ObservableCollection<AsyncBitmapImage>();
        ObservableCollection<CommentViewModel> CommentObservableCollection = new ObservableCollection<CommentViewModel>();
        FeedViewModel oneFeed;
        int PageIndex;
        bool shouldGoBack = true;

        bool _emergencyStop = false;

        public MorePicturesPage()
        {
            this.InitializeComponent();
            PicturesListView.SelectedIndex = -1;

            if (LoginHelper.LoggedIn == false)
            {
                CommentTextBox.IsEnabled = false;
                CommentTextBox.PlaceholderText = "要先登录才能评论哦";
                CommentButton.IsEnabled = false;
                LikeButton.IsEnabled = false;
                LikedButton.IsEnabled = false;
            }
            else
            {
                CommentTextBox.IsEnabled = true;
                CommentTextBox.PlaceholderText = "留下你的精彩评论吧";
                CommentButton.IsEnabled = true;
                LikeButton.IsEnabled = true;
                LikedButton.IsEnabled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter.GetType().Equals(typeof(FeedViewModel)))
                {
                    try
                    {
                        oneFeed = (FeedViewModel)e.Parameter;
                        shouldGoBack = oneFeed.ShouldGoBack;
                        _emergencyStop = false;
                        LoadPictures();
                    }
                    catch
                    { }
                    ReviewTextBlock.Text = oneFeed.Review == 0 ? "评论" : "评论(" + oneFeed.Review + ")";
                    GetComments(oneFeed.PostID, 1);
                    PageIndex = 2;
                }
                else
                {
                    this.Frame.GoBack();
                }
            }
            catch { }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                PageIndex = 1;
                _emergencyStop = true;
                allPictures.Clear();
                CommentObservableCollection.Clear();
                GC.Collect();
            }
            catch { }
        }

        private async void LoadPictures()
        {
            try
            {
                LoadingProgressBar.IsIndeterminate = true;
                foreach (var item in oneFeed.Pictures)
                {
                    if (_emergencyStop)
                    {
                        break;
                    }
                    await item.LoadImageAsync(196);
                    allPictures.Add(item);
                }
                LoadingProgressBar.IsIndeterminate = false;
            }
            catch { }
        }


        /// <summary>
        /// 获取当前图集的评论
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="page"></param>
        public async void GetComments(string postId, int page)
        {
            try
            {
                string para = postId + "/comments?page=" + page + "&count=15";
                Models.Comments.Comments comments = await TuchongApi.GetComments(para);
                if (comments == null || comments.result != "SUCCESS")
                {
                    NoCommentStackPanel.Visibility = Visibility.Visible;
                    NoCommentTextBlock.Text = "加载评论失败，请稍后重试";
                    CommentListView.Visibility = Visibility.Collapsed;
                    LoadMoreCommentsButton.Visibility = Visibility.Collapsed;
                    return;
                }
                foreach (var comment in comments.commentlist)
                {
                    CommentViewModel commentViewModel = new CommentViewModel(comment);
                    if (commentViewModel.InfoComplete)
                    {
                        CommentObservableCollection.Add(commentViewModel);
                    }
                }
                if (CommentObservableCollection.Count <= 0)
                {
                    NoCommentStackPanel.Visibility = Visibility.Visible;
                    NoCommentTextBlock.Text = "还没有人评论哦";
                    CommentListView.Visibility = Visibility.Collapsed;
                    LoadMoreCommentsButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    NoCommentStackPanel.Visibility = Visibility.Collapsed;
                    CommentListView.Visibility = Visibility.Visible;
                    if (comments.more == true)
                    {
                        LoadMoreCommentsButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        LoadMoreCommentsButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception e)
            {
                DialogShower.ShowDialog("加载评论异常", e.Message);
            }
        }

        /// <summary>
        /// 加载更多评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMoreCommentsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetComments(oneFeed.PostID, PageIndex);
                PageIndex++;
            }
            catch { }
        }

        /// <summary>
        /// 鼠标指向某一张图片的动画效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement s)
            {
                VisualHelper.SetScale(s, "1.1,1.1,1.1");
            }
        }

        /// <summary>
        /// 鼠标指向某一张图片的动画效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement s)
            {
                VisualHelper.SetScale(s, "1,1,1");
            }
        }

        /// <summary>
        /// 查看一张照片的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicturesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PicturesListView.SelectedIndex < 0)
            {
                return;
            }
            oneFeed.ReadingIndex = PicturesListView.SelectedIndex;
            this.Frame.Navigate(typeof(ImageDetailPage), oneFeed);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack && shouldGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {
                this.Frame.Navigate(typeof(BlankPage));
            }
        }

        /// <summary>
        /// 按下回车键发布评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommentTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                PostComment();
            }
        }

        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (CommentTextBox.Text.Length > 0)
            {
                PostComment();
            }
        }

        /// <summary>
        /// 喜欢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            bool? sent;
            try
            {
                if (oneFeed.isFavarite == Visibility.Collapsed)
                {
                    sent = await TuchongApi.PutFavorite(oneFeed.PostID);
                    if (sent == true)
                    {
                        oneFeed.isFavarite = Visibility.Visible;
                    }
                }
                else
                {
                    sent = await TuchongApi.DeleteFavorite(oneFeed.PostID);
                    if (sent == true)
                    {
                        oneFeed.isFavarite = Visibility.Collapsed;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 个人主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Helpers.MsgBus.Instance.PhotographerID = oneFeed.UserID;
            this.Frame.Navigate(typeof(PhotographerPage), true);
        }

        private async void PostComment()
        {
            string result = await TuchongApi.PostComment(oneFeed.PostID, CommentTextBox.Text);
            ShowToast(result);
            if (result.Contains("评论成功"))
            {
                CommentObservableCollection.Clear();
                GetComments(oneFeed.PostID, 1);
                PageIndex = 2;
            }
            CommentTextBox.Text = "";
        }

        private void ToastNotify_Completed(object sender, object e)
        {
            ToastNotifyDismiss.Begin();
        }

        private void ToastNotifyDismiss_Completed(object sender, object e)
        {
            NoteGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoginHelper.LoggedIn == false)
                {
                    ShowToast("请先登录哦");
                    return;
                }
                var resp = await TuchongApi.PutFollow(oneFeed.UserID.Substring(4));
                if (resp != null)
                {
                    if (resp.result == "SUCCESS")
                    {
                        oneFeed.isFollowing = true;
                    }
                    ShowToast(resp.message);
                }
                else
                {
                    ShowToast("关注失败，数据解析错误");
                }
            }
            catch (Exception ex)
            {
                DialogShower.ShowDialog("关注异常", ex.Message);
            }
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoginHelper.LoggedIn == false)
                {
                    ShowToast("请先登录哦");
                    return;
                }
                var resp = await TuchongApi.DeleteFollow(oneFeed.UserID.Substring(4));
                if (resp != null)
                {
                    if (resp.result == "SUCCESS")
                    {
                        oneFeed.isFollowing = false;
                    }
                    ShowToast(resp.message);
                }
                else
                {
                    ShowToast("取消关注失败，数据解析错误");
                }
            }
            catch (Exception ex)
            {
                DialogShower.ShowDialog("取关异常", ex.Message);
            }
        }

        private void ShowToast(string content)
        {
            NoteGrid.Visibility = Visibility.Visible;
            NoteTextBlock.Text = content;
            ToastNotify.Begin();
        }
    }
}
