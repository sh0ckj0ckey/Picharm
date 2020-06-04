using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using 图虫.Helpers;
using 图虫.ViewModels;

namespace 图虫
{
    public class FeedViewModel : INotifyPropertyChanged
    {
        public string UserPhotoUrl { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string UserDesc { get; set; }
        public string PostID { get; set; }
        public string PostDate { get; set; }
        public string FirstPicUrl { get; set; }
        public BitmapImage FirstPic { get; set; }
        public int PicsCount { get; set; } = 0;
        public Visibility MultiPics { get; set; } = Visibility.Collapsed;
        public Visibility HasDescription { get; set; } = Visibility.Collapsed;
        public Visibility HasTags { get; set; } = Visibility.Collapsed;

        private double _like = 0;
        public double Like
        {
            get { return this._like; }
            set
            {
                this._like = value;
                NotifyPropertyChanged();
            }
        }

        public double Review { get; set; } = 0;
        public double Share { get; set; } = 0;
        public string Description { get; set; }
        public ObservableCollection<string> Tags { get; set; } = new ObservableCollection<string>();
        public string TagsDesc { get; set; }
        public bool InfoComplete { get; set; } = false;
        public ObservableCollection<AsyncBitmapImage> Pictures { get; set; } = new ObservableCollection<AsyncBitmapImage>();
        public List<string> PicturesDesc { get; set; } = new List<string>();
        public List<string> PicturesTitle { get; set; } = new List<string>();
        public List<string> PicturesID { get; set; } = new List<string>();

        private Visibility _isFavorite = Visibility.Collapsed;
        public Visibility isFavarite
        {
            get { return this._isFavorite; }
            set
            {
                this._isFavorite = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isFollowing = false;
        public bool isFollowing
        {
            get { return this._isFollowing; }
            set
            {
                this._isFollowing = value;
                NotifyPropertyChanged();
            }
        }

        // 目前只在“我的喜欢”里用到了这个属性
        public string Title { get; set; }

        // 标识当前 ViewModel 是从哪里传递的，
        // 如果是 True 说明是从摄影师资料页传递到 MorePicturesPage 的，那么返回的时候要返回上一页
        // 如果是 False 说明是从列表点击传递的，那么返回时直接回到 BlankPage 即可
        public bool ShouldGoBack { get; set; } = true;

        // 这个属性用于在MorePicturesPage到ImageDetailPage传递参数时标记用户点击的是第几张图片
        public int ReadingIndex { get; set; } = 0;

        public async Task LoadImageAsync(int decodeWidth = 412)
        {
            FirstPic = await ImageLoader.LoadImageAsync(FirstPicUrl);
            FirstPic.DecodePixelType = DecodePixelType.Logical;
            FirstPic.DecodePixelWidth = decodeWidth;
        }

        ~FeedViewModel()
        {
            this.Tags?.Clear();
            this.Pictures?.Clear();
            this.PicturesDesc?.Clear();
            this.PicturesTitle?.Clear();
            this.PicturesID?.Clear();
            this.Tags = null;
            this.Pictures = null;
            this.PicturesDesc = null;
            this.PicturesTitle = null;
            this.PicturesID = null;
        }

        public FeedViewModel(Models.Feeds.Feedlist feedlist)
        {
            this.InfoComplete = false;
            if (feedlist.type == "post")
            {
                try
                {
                    this.UserPhotoUrl = feedlist.entry.site.icon;
                    this.UserName = feedlist.entry.site.name;
                    this.UserID = "ID: " + feedlist.entry.site.site_id;
                    this.UserDesc = feedlist.entry.site.description != "" ? feedlist.entry.site.description : "他注定是个低调的大侠，所以什么都没写";
                    this.PostID = feedlist.entry.post_id.ToString();
                    this.PostDate = feedlist.entry.published_at.Substring(0, 10).Trim();
                    this.FirstPicUrl = "https://photo.tuchong.com/" + feedlist.entry.images[0].user_id + "/f/" + feedlist.entry.images[0].img_id + ".jpg";
                    this.PicsCount = feedlist.entry.images.Length;
                    this.MultiPics = (feedlist.entry.images.Length > 1) ? Visibility.Visible : Visibility.Collapsed;
                    this.Like = feedlist.entry.favorites;
                    this.Review = feedlist.entry.comments;
                    this.Share = feedlist.entry.shares;
                    this.Description = feedlist.entry.content.Replace("<br />", "");
                    this.HasDescription = (feedlist.entry.content.Trim() == "") ? Visibility.Collapsed : Visibility.Visible;
                    this.Tags = new ObservableCollection<string>();
                    this.TagsDesc = "";
                    try
                    {
                        foreach (var item in feedlist.entry.tags)
                        {
                            this.Tags.Add("#" + item.tag_name);
                            this.TagsDesc += ("#" + item.tag_name + "  ");
                        }
                    }
                    catch { }
                    this.HasTags = (feedlist.entry.tags.Length == 0) ? Visibility.Collapsed : Visibility.Visible;
                    this.isFavarite = feedlist.entry.is_favorite == true ? Visibility.Visible : Visibility.Collapsed;
                    this.isFollowing = feedlist.entry.site.is_following;
                    this.Pictures = new ObservableCollection<AsyncBitmapImage>();
                    this.PicturesDesc = new List<string>();
                    this.PicturesTitle = new List<string>();
                    this.PicturesID = new List<string>();
                    try
                    {
                        foreach (var item in feedlist.entry.images)
                        {
                            this.Pictures.Add(new AsyncBitmapImage() { ImageUri = "https://photo.tuchong.com/" + item.user_id + "/f/" + item.img_id + ".jpg" });
                            this.PicturesDesc.Add(item.description);
                            this.PicturesTitle.Add(item.title);
                            this.PicturesID.Add(item.img_id.ToString());
                        }
                    }
                    catch { }
                    this.InfoComplete = true;
                }
                catch
                {
                    this.InfoComplete = false;
                }
            }
        }

        public FeedViewModel(Models.CategoryItem.Post_List postlist)
        {
            this.InfoComplete = false;
            try
            {
                this.UserPhotoUrl = postlist.site.icon;
                this.UserName = postlist.site.name;
                this.UserID = "ID: " + postlist.site.site_id;
                this.UserDesc = postlist.site.description != "" ? postlist.site.description : "他注定是个低调的大侠，所以什么都没写";
                this.PostID = postlist.post_id.ToString();
                this.PostDate = postlist.published_at.Substring(0, 10).Trim();
                this.FirstPicUrl = "https://photo.tuchong.com/" + postlist.images[0].user_id + "/f/" + postlist.images[0].img_id + ".jpg";
                this.PicsCount = postlist.images.Length;
                this.MultiPics = (postlist.images.Length > 1) ? Visibility.Visible : Visibility.Collapsed;
                this.Like = postlist.favorites;
                this.Review = postlist.comments;
                this.Share = postlist.shares;
                this.Description = postlist.content.Replace("<br />", "");
                this.HasDescription = (postlist.content.Trim() == "") ? Visibility.Collapsed : Visibility.Visible;
                this.Tags = new ObservableCollection<string>();
                this.TagsDesc = "";
                try
                {
                    foreach (var item in postlist.tags)
                    {
                        this.Tags.Add("#" + item.tag_name);
                        this.TagsDesc += ("#" + item.tag_name + "  ");
                    }
                }
                catch { }
                this.HasTags = (postlist.tags.Length == 0) ? Visibility.Collapsed : Visibility.Visible;
                this.isFavarite = postlist.is_favorite == true ? Visibility.Visible : Visibility.Collapsed;
                this.isFollowing = postlist.site.is_following;
                this.Pictures = new ObservableCollection<AsyncBitmapImage>();
                this.PicturesDesc = new List<string>();
                this.PicturesTitle = new List<string>();
                this.PicturesID = new List<string>();
                try
                {
                    foreach (var item in postlist.images)
                    {
                        this.Pictures.Add(new AsyncBitmapImage() { ImageUri = "https://photo.tuchong.com/" + postlist.images[0].user_id + "/f/" + item.img_id + ".jpg" });
                        this.PicturesDesc.Add(item.description);
                        this.PicturesTitle.Add(item.title);
                        this.PicturesID.Add(item.img_id.ToString());
                    }
                }
                catch { }
                this.InfoComplete = true;
            }
            catch
            {
                this.InfoComplete = false;
            }
        }

        public FeedViewModel(PhotographViewModel photograph)
        {
            this.InfoComplete = false;
            try
            {
                this.UserPhotoUrl = photograph.UserPhotoUrl;
                this.UserName = photograph.UserName;
                this.UserID = photograph.UserID;
                this.UserDesc = photograph.UserDesc;
                this.PostID = photograph.PostID;
                this.FirstPicUrl = photograph.FirstPicUrl;
                this.PicsCount = photograph.PicsCount;
                this.Tags = photograph.Tags;
                this.HasTags = photograph.HasTags;
                this.Pictures = photograph.Pictures;
                this.PicturesDesc = photograph.PicturesDesc;
                this.PicturesTitle = photograph.PicturesTitle;
                this.PicturesID = photograph.PicturesID;
                this.isFavarite = photograph.isFavorite;
                this.isFollowing = photograph.isFollowing;
                this.Description = photograph.Description;
                this.HasDescription = (photograph.Description.Trim() == "") ? Visibility.Collapsed : Visibility.Visible;
                this.Like = photograph.LikeCount;
                this.InfoComplete = true;
            }
            catch
            {
                this.InfoComplete = false;
            }
        }

        public FeedViewModel(Models.PostsSearchResult.Post_List posts)
        {
            this.InfoComplete = false;
            try
            {
                this.UserPhotoUrl = posts.site.icon;
                this.UserName = posts.site.name;
                this.UserID = "ID: " + posts.site.site_id;
                this.UserDesc = posts.site.description != "" ? posts.site.description : "他注定是个低调的大侠，所以什么都没写";
                this.PostID = posts.post_id.ToString();
                this.FirstPicUrl = posts.images[0].source.f; //posts.title_image.url;
                this.PicsCount = posts.images.Length;
                this.Tags = new ObservableCollection<string>();
                try
                {
                    foreach (var item in posts.tags)
                    {
                        this.Tags.Add("#" + item.tag_name);
                    }
                }
                catch { }
                this.Description = posts.excerpt;
                this.HasDescription = (posts.excerpt.Trim() == "") ? Visibility.Collapsed : Visibility.Visible;
                this.Like = posts.favorites;
                this.HasTags = (posts.tags.Length == 0) ? Visibility.Collapsed : Visibility.Visible;
                this.isFavarite = posts.is_favorite == true ? Visibility.Visible : Visibility.Collapsed;
                this.isFollowing = posts.site.is_following;
                this.Pictures = new ObservableCollection<AsyncBitmapImage>();
                this.PicturesDesc = new List<string>();
                this.PicturesTitle = new List<string>();
                this.PicturesID = new List<string>();
                try
                {
                    foreach (var item in posts.images)
                    {
                        this.Pictures.Add(new AsyncBitmapImage() { ImageUri = item.source.f });
                        this.PicturesDesc.Add(item.excerpt);
                        this.PicturesTitle.Add(item.title);
                        this.PicturesID.Add(item.img_id.ToString());
                    }
                }
                catch { }
                this.InfoComplete = true;
            }
            catch
            {
                this.InfoComplete = false;
            }
        }

        public FeedViewModel(Models.MyLike.Entry entry)
        {
            this.InfoComplete = false;
            try
            {
                this.UserPhotoUrl = entry.site.icon;
                this.UserName = entry.site.name;
                this.UserID = "ID: " + entry.site.site_id;
                this.UserDesc = entry.site.description != "" ? entry.site.description : "他注定是个低调的大侠，所以什么都没写";
                this.PostID = entry.post_id.ToString();
                this.FirstPicUrl = entry.images[0].source.f;
                this.PicsCount = entry.images.Length;
                this.Description = entry.excerpt;
                this.HasDescription = (entry.excerpt.Trim() == "") ? Visibility.Collapsed : Visibility.Visible;
                this.Like = entry.favorites;
                this.Tags = new ObservableCollection<string>();
                try
                {
                    foreach (var item in entry.tags)
                    {
                        this.Tags.Add("#" + item.tag_name);
                    }
                }
                catch { }
                this.HasTags = (entry.tags.Length == 0) ? Visibility.Collapsed : Visibility.Visible;
                this.isFavarite = entry.is_favorite == true ? Visibility.Visible : Visibility.Collapsed;
                this.isFollowing = entry.site.is_following;
                this.Title = entry.title;
                this.Pictures = new ObservableCollection<AsyncBitmapImage>();
                this.PicturesDesc = new List<string>();
                this.PicturesTitle = new List<string>();
                this.PicturesID = new List<string>();
                try
                {
                    foreach (var item in entry.images)
                    {
                        this.Pictures.Add(new AsyncBitmapImage() { ImageUri = item.source.f });
                        this.PicturesDesc.Add(item.excerpt);
                        this.PicturesTitle.Add(item.title);
                        this.PicturesID.Add(item.img_id.ToString());
                    }
                }
                catch { }
                this.InfoComplete = true;
            }
            catch
            {
                this.InfoComplete = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
