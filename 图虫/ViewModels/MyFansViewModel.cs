using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace 图虫.ViewModels
{
    public class MyFansViewModel : ViewModelBase
    {
        /// <summary>
        /// 用户头像
        /// </summary>
        private BitmapImage _avatar;
        public BitmapImage Avatar
        {
            get => _avatar;
            set => Set(ref _avatar, value);
        }

        private Visibility _hasMore = Visibility.Visible;
        public Visibility HasMore
        {
            get => _hasMore;
            set => Set(ref _hasMore, value);
        }

        private Visibility _end = Visibility.Collapsed;
        public Visibility End
        {
            get => _end;
            set => Set(ref _end, value);
        }

        private ObservableCollection<Models.MyFans.Site> _fansList = new ObservableCollection<Models.MyFans.Site>();
        public ObservableCollection<Models.MyFans.Site> FansList
        {
            get => _fansList;
            set => Set(ref _fansList, value);
        }

        public void ResetData()
        {
            FansList.Clear();
            HasMore = Visibility.Visible;
            End = Visibility.Collapsed;
        }
    }
}
