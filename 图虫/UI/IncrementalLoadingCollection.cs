using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

        //the count is the number requested
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(async cancelToken =>
            {
                Models.Feeds.Feed gottenFeeds = null;
                if (this.feedPageIndex == 1)
                {
                    gottenFeeds = await ApiHelper.GetFeed("page=1&type=refresh");
                    if (gottenFeeds == null || gottenFeeds.message.StartsWith("获取或者解析数据失败"))
                    {
                        this.feedPageIndex = 1;
                        if (gottenFeeds != null)
                        {
                            ShowDialog(gottenFeeds.message);
                        }
                    }
                    else if (gottenFeeds.result != "SUCCESS")
                    {
                        this.feedPageIndex = 1;
                        ShowDialog(gottenFeeds.result + ": " + gottenFeeds.message);
                    }
                    else
                    {
                        HasMoreItems = gottenFeeds.more;
                        if (gottenFeeds.feedList.Length <= 0)
                        {
                            this.feedPageIndex = 1;
                        }
                        else
                        {
                            foreach (var feed in gottenFeeds.feedList)
                            {
                                FeedViewModel feedViewModel = new FeedViewModel(feed);
                                if (feedViewModel.InfoComplete)
                                {
                                    this.Add(feedViewModel);
                                }

                                if (feed.entry.post_id != 0)
                                {
                                    this.feedPageLastImgID = feed.entry.post_id.ToString();
                                }
                                else if (feed.entry.vid != 0)
                                {
                                    this.feedPageLastImgID = feed.entry.vid.ToString();
                                }
                            }
                            this.feedPageIndex += 1;
                            Loading = Visibility.Collapsed;
                            isLoading = false;
                        }
                    }
                }
                else
                {
                    //add your newly loaded item to the collection
                    if (this.feedPageLastImgID == null || this.feedPageLastImgID == "")
                    {
                        this.feedPageIndex = 1;
                    }
                    else
                    {
                        gottenFeeds = await ApiHelper.GetFeed("page=" + this.feedPageIndex + "post_id=" + this.feedPageLastImgID + "&type=loadmore");
                        if (gottenFeeds == null || gottenFeeds.message.StartsWith("获取或者解析数据失败"))
                        {
                            if (gottenFeeds != null)
                            {
                                ShowDialog(gottenFeeds.message);
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
                                        this.Add(feedViewModel);
                                    }

                                    if (feed.entry.post_id != 0)
                                    {
                                        this.feedPageLastImgID = feed.entry.post_id.ToString();
                                    }
                                    else if (feed.entry.vid != 0)
                                    {
                                        this.feedPageLastImgID = feed.entry.vid.ToString();
                                    }
                                }
                                this.feedPageIndex += 1;
                            }
                        }
                    }
                }
                Loading = Visibility.Collapsed;
                isLoading = false;
                return new LoadMoreItemsResult { Count = count };
            });
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
