using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using 图虫.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        ObservableCollection<SitesSearchViewModel> SitesCollection = new ObservableCollection<SitesSearchViewModel>();
        ObservableCollection<TagsSearchViewModel> TagsCollection = new ObservableCollection<TagsSearchViewModel>();
        ObservableCollection<PostsSearchViewModel> PostsCollection = new ObservableCollection<PostsSearchViewModel>();

        bool _emergencyStop = false;
        string search = "";

        public SearchPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //ConnectedAnimation animation =
            //    ConnectedAnimationService.GetForCurrentView().GetAnimation("goSearchAnimation");
            //if (animation != null)
            //{
            //    animation.TryStart(SearchTextBox, new UIElement[] { SearchButton });
            //}
            try
            {
                _emergencyStop = false;

                if (e.Parameter.GetType().Equals(typeof(string)))
                {
                    try
                    {
                        if (search != (string)e.Parameter)
                        {
                            search = (string)e.Parameter;
                            SearchingTextBlock.Text = "搜索结果: " + search;
                            StartSearch(search);
                        }
                    }
                    catch { }
                }
            }
            catch { }
            base.OnNavigatedTo(e);
        }

        //protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        //{
        //    if (e.NavigationMode == NavigationMode.Back)
        //    {
        //        ConnectedAnimation animation =
        //            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backAnimation", SearchTextBox);

        //        // Use the recommended configuration for back animation.
        //        animation.Configuration = new DirectConnectedAnimationConfiguration();
        //    }
        //}


        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _emergencyStop = true;
                SitesCollection?.Clear();
                TagsCollection?.Clear();
                PostsCollection?.Clear();
                GC.Collect();
            }
            catch { }
            //if (this.Frame.CanGoBack)
            //{
            //    this.Frame.GoBack();
            //}
            //else
            //{
            this.Frame.Navigate(typeof(BlankPage));
            //}
        }

        ///// <summary>
        ///// 搜索
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SearchTextBox.Text.Trim() != "")
        //    {
        //        StartSearch(SearchTextBox.Text);
        //    }
        //}

        ///// <summary>
        ///// 按下回车搜索
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void SearchTextBox_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        //{
        //    if (SearchTextBox.Text.Trim() != "")
        //    {
        //        e.Handled = true;
        //        if (e.Key == Windows.System.VirtualKey.Enter)
        //        {
        //            StartSearch(SearchTextBox.Text);
        //        }
        //    }
        //}

        private async void StartSearch(string query)
        {
            try
            {
                EmptyGrid.Visibility = Visibility.Collapsed;
                SearchingProgressBar.IsIndeterminate = true;

                SitesTitleGrid.Visibility = Visibility.Collapsed;
                SitesListGrid.Visibility = Visibility.Collapsed;
                TagsTitleGrid.Visibility = Visibility.Collapsed;
                TagsListGrid.Visibility = Visibility.Collapsed;
                PostsTitleGrid.Visibility = Visibility.Collapsed;
                PostsListGrid.Visibility = Visibility.Collapsed;

                SitesCollection.Clear();
                TagsCollection.Clear();
                PostsCollection.Clear();

                var sites = await TuchongApi.GetSitesSearchResult(query);
                if (sites != null && sites.data != null && sites.data.site_list != null & sites.data.site_list.Length > 0)
                {
                    SitesTitleGrid.Visibility = Visibility.Visible;
                    SitesListGrid.Visibility = Visibility.Visible;
                    foreach (var item in sites.data.site_list)
                    {
                        if (_emergencyStop)
                        {
                            break;
                        }
                        var site = new SitesSearchViewModel(item);
                        await site.LoadImageAsync();
                        SitesCollection.Add(site);
                    }
                }

                var tags = await TuchongApi.GetTagsSearchResult(query);
                if (tags != null && tags.data != null && tags.data.tag_list != null & tags.data.tag_list.Length > 0)
                {
                    TagsTitleGrid.Visibility = Visibility.Visible;
                    TagsListGrid.Visibility = Visibility.Visible;
                    foreach (var item in tags.data.tag_list)
                    {
                        if (_emergencyStop)
                        {
                            break;
                        }
                        var tag = new TagsSearchViewModel(item);
                        await tag.LoadImageAsync();
                        TagsCollection.Add(tag);
                    }
                }

                var posts = await TuchongApi.GetPostsSearchResult(query);
                if (posts != null && posts.data != null && posts.data.post_list != null & posts.data.post_list.Length > 0)
                {
                    PostsTitleGrid.Visibility = Visibility.Visible;
                    PostsListGrid.Visibility = Visibility.Visible;
                    foreach (var item in posts.data.post_list)
                    {
                        if (_emergencyStop)
                        {
                            break;
                        }
                        var post = new PostsSearchViewModel(item);
                        await post.LoadImageAsync();
                        PostsCollection.Add(post);
                    }
                }

                if (SitesCollection.Count <= 0 && TagsCollection.Count <= 0 && PostsCollection.Count <= 0)
                {
                    EmptyGrid.Visibility = Visibility.Visible;
                }

                SearchingProgressBar.IsIndeterminate = false;
            }
            catch { }
            //SearchResultPopIn.Begin();
        }

        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    SearchTextBox.Focus(FocusState.Keyboard);
        //}

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppBarButton bt = sender as AppBarButton;
                SitesSearchViewModel site = (SitesSearchViewModel)bt.DataContext;
                Helpers.MsgBus.Instance.PhotographerID = "ID: " + site.Id;
                this.Frame.Navigate(typeof(PhotographerPage), true);
            }
            catch { }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                AppBarButton bt = sender as AppBarButton;
                TagsSearchViewModel tag = (TagsSearchViewModel)bt.DataContext;
                await Windows.System.Launcher.LaunchUriAsync(new Uri(tag.Url));
            }
            catch { }
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bt = sender as Button;
                PostsSearchViewModel post = (PostsSearchViewModel)bt.DataContext;
                var para = new FeedViewModel(post.PostRaw);
                para.ShouldGoBack = true;
                this.Frame.Navigate(typeof(MorePicturesPage), para);
            }
            catch { }
        }
    }
}
