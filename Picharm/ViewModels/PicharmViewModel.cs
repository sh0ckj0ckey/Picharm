using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Picharm.Helpers;
using Picharm.Models;

namespace Picharm.ViewModels
{
    internal class PicharmViewModel : ObservableObject
    {
        private static Lazy<PicharmViewModel> _lazyVM = new Lazy<PicharmViewModel>(() => new PicharmViewModel());
        public static PicharmViewModel Instance => _lazyVM.Value;

        public SettingsService AppSettings { get; set; } = new SettingsService();

        /// <summary>
        /// 刷新页面事件
        /// </summary>
        public event EventHandler RefreshEvent;

        public ObservableCollection<MainNavigationBase> MainNavigationItems = new ObservableCollection<MainNavigationBase>();

        public ObservableCollection<MainNavigationBase> MainNavigationFooterItems = new ObservableCollection<MainNavigationBase>();

        public PicharmViewModel()
        {
            MainNavigationItems.Add(new MainNavigationHeader("推荐"));
            MainNavigationItems.Add(new MainNavigationItem("社区", AppPagesEnum.Homepage.ToString(), "\uECA5"));
            MainNavigationItems.Add(new MainNavigationItem("活动", AppPagesEnum.Discover.ToString(), "\uECAD"));
            MainNavigationItems.Add(new MainNavigationHeader("我的"));
            MainNavigationItems.Add(new MainNavigationItem("喜欢", AppPagesEnum.MyLike.ToString(), "\uEB51"));
            MainNavigationItems.Add(new MainNavigationItem("关注", AppPagesEnum.MyFollow.ToString(), "\uE716"));
            MainNavigationItems.Add(new MainNavigationItem("粉丝", AppPagesEnum.MyFans.ToString(), "\uF131"));
            MainNavigationFooterItems.Add(new MainNavigationUserItem(AppPagesEnum.Login.ToString()));
            MainNavigationFooterItems.Add(new MainNavigationSeparator());
            MainNavigationFooterItems.Add(new MainNavigationSettingItem(AppPagesEnum.Settings.ToString()));
        }
    }
}
