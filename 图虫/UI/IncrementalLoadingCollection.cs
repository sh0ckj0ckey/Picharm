using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace 图虫
{
    public class IncrementalLoadingCollection : ObservableCollection<FeedViewModel>, ISupportIncrementalLoading
    {
        //标记当前获取的第几页Feed
        uint feedPageIndex = 1;

        //上一页最后一张图片的ID存起来，获取下一页时会用到
        string feedPageLastImgID;

        public bool HasMoreItems { get; set; } = true;

        private Visibility _loading = Visibility.Visible;
        public Visibility Loading
        {
            get { return this._loading; }
            set
            {
                this._loading = value;
            }
        }

        private bool _isLoading = true;
        public bool isLoading
        {
            get { return this._isLoading; }
            set
            {
                this._isLoading = value;
            }
        }

        bool _busy = false;

        //the count is the number requested
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                Models.Feeds.Feed gottenFeeds = null;
                if (!_busy)
                {
                    _busy = true;
                    if (this.feedPageIndex == 1)
                    {
                        gottenFeeds = await TuchongApi.GetFeed("page=1&type=refresh");
                        Loading = Visibility.Collapsed;
                        isLoading = false;
                        if (gottenFeeds == null || gottenFeeds.message.StartsWith("获取或者解析数据失败") || gottenFeeds.result != "SUCCESS")
                        {
                            this.feedPageIndex = 1;
                            if (gottenFeeds != null)
                            {
                                ShowDialog(gottenFeeds.result + ": " + gottenFeeds.message);
                            }
                        }
                        else
                        {
                            HasMoreItems = gottenFeeds.more;
                            if (gottenFeeds.feedList.Length <= 0)
                            {
                                this.feedPageIndex = 1;
                                ShowDialog("没有获取到作品，请刷新重试");
                            }
                            else
                            {
                                foreach (var feed in gottenFeeds.feedList)
                                {
                                    FeedViewModel feedViewModel = new FeedViewModel(feed);
                                    if (feedViewModel.InfoComplete)
                                    {
                                        await feedViewModel.LoadImageAsync();
                                        this.Add(feedViewModel);
                                    }

                                    if (feed.entry.post_id != null & feed.entry.post_id != "0")
                                    {
                                        this.feedPageLastImgID = feed.entry.post_id.ToString();
                                    }
                                    else if (feed.entry.vid != null & feed.entry.vid != "0")
                                    {
                                        this.feedPageLastImgID = feed.entry.vid.ToString();
                                    }
                                    else if (feed.entry.video_id != null & feed.entry.video_id != "0")
                                    {
                                        this.feedPageLastImgID = feed.entry.video_id.ToString();
                                    }
                                }
                                this.feedPageIndex += 1;
                            }
                        }
                    }
                    else
                    {
                        if (this.feedPageLastImgID == null || this.feedPageLastImgID == "")
                        {
                            this.feedPageIndex = 1;
                        }
                        else
                        {
                            gottenFeeds = await TuchongApi.GetFeed("page=" + this.feedPageIndex + "&post_id=" + this.feedPageLastImgID + "&type=loadmore");
                            Loading = Visibility.Collapsed;
                            isLoading = false;
                            if (gottenFeeds == null || gottenFeeds.message.StartsWith("获取或者解析数据失败") || gottenFeeds.result != "SUCCESS")
                            {
                                if (gottenFeeds != null)
                                {
                                    ShowDialog(gottenFeeds.result + ": " + gottenFeeds.message);
                                }
                            }
                            else
                            {
                                HasMoreItems = gottenFeeds.more;
                                if (gottenFeeds.feedList.Length > 0)
                                {
                                    foreach (var feed in gottenFeeds.feedList)
                                    {
                                        FeedViewModel feedViewModel = new FeedViewModel(feed);
                                        if (feedViewModel.InfoComplete)
                                        {
                                            await feedViewModel.LoadImageAsync();
                                            this.Add(feedViewModel);
                                        }

                                        if (feed.entry.post_id != null & feed.entry.post_id != "0")
                                        {
                                            this.feedPageLastImgID = feed.entry.post_id.ToString();
                                        }
                                        else if (feed.entry.vid != null & feed.entry.vid != "0")
                                        {
                                            this.feedPageLastImgID = feed.entry.vid.ToString();
                                        }
                                        else if (feed.entry.video_id != null & feed.entry.video_id != "0")
                                        {
                                            this.feedPageLastImgID = feed.entry.video_id.ToString();
                                        }
                                    }
                                    this.feedPageIndex += 1;
                                }
                            }
                        }
                    }
                }
                return new LoadMoreItemsResult { Count = 0 };
            }
            finally
            {
                _busy = false;
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshData()
        {
            if (isLoading)
            {
                return;
            }
            _busy = false;
            feedPageIndex = 1;
            feedPageLastImgID = "";
            HasMoreItems = true;
            Loading = Visibility.Visible;
            isLoading = true;
            this.ClearItems();
        }

        /// <summary>
        /// 显示程序异常对话框，自定义内容
        /// </summary>
        public static async void ShowDialog(string content)
        {
            var dialog = new Windows.UI.Xaml.Controls.ContentDialog()
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

    }

}
