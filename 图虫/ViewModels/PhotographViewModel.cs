using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using 图虫.Models.Photograph;

namespace 图虫.ViewModels
{
    public class PhotographViewModel : ViewModelBase
    {
        public string UserPhotoUrl { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string UserDesc { get; set; }
        public string PostID { get; set; }
        public string FirstPicUrl { get; set; }
        public int PicsCount { get; set; }
        public string Description { get; set; }
        public double LikeCount { get; set; } = 0;
        public Visibility HasTags { get; set; }

        private Visibility _isFavorite = Visibility.Collapsed;
        public Visibility isFavorite
        {
            get => _isFavorite;
            set => Set(ref _isFavorite, value);
        }

        private bool _isFollowing = false;
        public bool isFollowing
        {
            get => _isFollowing;
            set => Set(ref _isFollowing, value);
        }

        public ObservableCollection<string> Tags { get; set; }
        public bool InfoComplete { get; set; }
        public ObservableCollection<AsyncBitmapImage> Pictures { get; set; }
        public List<string> PicturesDesc { get; set; }
        public List<string> PicturesTitle { get; set; }
        public List<string> PicturesID { get; set; }


        /// <summary>
        /// 这个属性用于在 MorePicturesPage 到 ImageDetailPage 传递参数时标记用户点击的是第几张图片
        /// </summary>
        public int ReadingIndex { get; set; }

        public PhotographViewModel(Post_List postlist)
        {
            this.InfoComplete = false;
            try
            {
                this.UserPhotoUrl = postlist.site.icon;
                this.UserName = postlist.site.name;
                this.UserID = "ID: " + postlist.site.site_id;
                this.UserDesc = postlist.site.description != "" ? postlist.site.description : "他注定是个低调的大侠，所以什么都没写";
                this.PostID = postlist.post_id.ToString();
                this.FirstPicUrl = "https://photo.tuchong.com/" + postlist.images[0].user_id + "/f/" + postlist.images[0].img_id + ".jpg";
                this.PicsCount = postlist.images.Length;
                this.Tags = new ObservableCollection<string>();
                try
                {
                    foreach (var item in postlist.tags)
                    {
                        this.Tags.Add("#" + item.tag_name);
                    }
                }
                catch { }
                this.Description = postlist.excerpt;
                this.HasTags = (postlist.tags.Length == 0) ? Visibility.Collapsed : Visibility.Visible;
                this.Pictures = new ObservableCollection<AsyncBitmapImage>();
                this.PicturesDesc = new List<string>();
                this.PicturesTitle = new List<string>();
                this.PicturesID = new List<string>();
                this.isFavorite = postlist.is_favorite ? Visibility.Visible : Visibility.Collapsed;
                this.isFollowing = postlist.site.is_following;
                this.LikeCount = postlist.favorites;
                try
                {
                    foreach (var item in postlist.images)
                    {
                        this.Pictures.Add(new AsyncBitmapImage() { ImageUri = "https://photo.tuchong.com/" + postlist.images[0].user_id + "/f/" + item.img_id + ".jpg" });
                        this.PicturesDesc.Add("");
                        this.PicturesTitle.Add(item.title);
                        this.PicturesID.Add(item.img_id);
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
}
