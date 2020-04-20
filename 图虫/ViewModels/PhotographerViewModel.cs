using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace 图虫.ViewModels
{
    public class PhotographerViewModel : ViewModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        private string _ID;
        public string ID
        {
            get => _ID;
            set => Set(ref _ID, value);
        }

        /// <summary>
        /// 名字
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        /// <summary>
        /// 位置
        /// </summary>
        private string _location;
        public string Location
        {
            get => _location;
            set => Set(ref _location, value);
        }

        /// <summary>
        /// 关注
        /// </summary>
        private int _following;
        public int Following
        {
            get => _following;
            set => Set(ref _following, value);
        }

        /// <summary>
        /// 粉丝
        /// </summary>
        private int _followers;
        public int Followers
        {
            get => _followers;
            set => Set(ref _followers, value);
        }

        /// <summary>
        /// 介绍
        /// </summary>
        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        /// <summary>
        /// 头像
        /// </summary>
        private ImageSource _icon;
        public ImageSource Icon
        {
            get => _icon;
            set => Set(ref _icon, value);
        }

        /// <summary>
        /// 是否关注
        /// </summary>
        private bool _isFollowing = false;
        public bool isFollowing
        {
            get => _isFollowing;
            set => Set(ref _isFollowing, value);
        }

        public PhotographerViewModel(Models.Photographer.PhotographerInfo info)
        {
            this.ID = info.site.site_id;
            this.Name = info.site.name;
            this.Location = info.site.user_location.Length == 0 ? "火星" : info.site.user_location;
            this.Followers = info.site.followers;
            this.Following = info.site.following;
            this.Description = info.site.intro;
            this.Icon = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(info.site.icon.Replace("avatar/l", "avatar/ll"), UriKind.Absolute));
            this.isFollowing = info.site.is_following;
        }

        public PhotographerViewModel() { }
    }
}
