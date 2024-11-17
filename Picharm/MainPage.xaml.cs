using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Picharm.ViewModels;
using Picharm.Views;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.UI.Core;
using Picharm.Models;
using System.Diagnostics;
using Picharm.Helpers;
using System.Reflection;
using CommunityToolkit.WinUI.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Picharm
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // 导航栏项的Tag对应的Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            (AppPagesEnum.Homepage.ToString(), typeof(HomePage)),
            (AppPagesEnum.Discover.ToString(), typeof(DiscoverPage)),
            (AppPagesEnum.PhotographyInfo.ToString(), typeof(PhotographyInfoPage)),
            (AppPagesEnum.PhotographyView.ToString(), typeof(PhotographyViewPage)),
            (AppPagesEnum.Photographer.ToString(), typeof(PhotographerPage)),
            (AppPagesEnum.MyLike.ToString(), typeof(MyLikePage)),
            (AppPagesEnum.MyFans.ToString(), typeof(MyFansPage)),
            (AppPagesEnum.MyFollow.ToString(), typeof(MyFollowPage)),
            (AppPagesEnum.Login.ToString(), typeof(LoginPage)),
            (AppPagesEnum.Search.ToString(), typeof(SearchPage)),
            (AppPagesEnum.Settings.ToString(), typeof(SettingsPage)),
        };

        private PicharmViewModel _viewModel = null;

        private ThemeListener _themeListener = null;

        public MainPage()
        {
            _viewModel = PicharmViewModel.Instance;

            this.InitializeComponent();

            _themeListener = new ThemeListener();
            _themeListener.ThemeChanged += (sender) =>
            {
                UpdateAppTheme();
            };

            _viewModel.AppSettings.AppearanceSettingChanged += (theme) =>
            {
                UpdateAppTheme();
            };

            _viewModel.AppSettings.BackdropSettingChanged += (material) =>
            {
                UpadateAppBackdrop();
            };
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetTitleBarArea();
                UpdateAppTheme();
                UpadateAppBackdrop();
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void MainNavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 页面发生导航时，更新侧边栏的选中项
                MainFrame.Navigated += MainFrame_Navigated;

                MainFramNavigateToPage(AppPagesEnum.Homepage.ToString(), null, new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());

                // 处理系统的返回键
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
                SystemNavigationManager.GetForCurrentView().BackRequested += System_BackRequested;
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void MainNavigationView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            try
            {
                if (args?.InvokedItemContainer?.Tag == null || string.IsNullOrWhiteSpace(args?.InvokedItemContainer?.Tag?.ToString())) return;

                if (args.InvokedItemContainer != null)
                {
                    var navItemTag = args.InvokedItemContainer.Tag.ToString();
                    MainFramNavigateToPage(navItemTag, args.RecommendedNavigationTransitionInfo);
                }

                // 清除返回
                MainFrame.BackStack.Clear();
                MainFrame.ForwardStack.Clear();
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            try
            {
                if (MainFrame.SourcePageType != null)
                {
                    string tag = (_pages.FirstOrDefault(p => p.Page == e.SourcePageType)).Tag;

                    //if (tag == "lives")
                    //{
                    //    if (LivesViewModel.Instance.iCurGameId >= 0)
                    //    {
                    //        tag = $"lives|{LivesViewModel.Instance.iCurGameId}";
                    //    }
                    //}

                    // 遍历侧栏找到匹配的选项，将侧栏的选中项对应到当前页面
                    MainNavigationBase select = null;
                    if (select is null)
                    {
                        foreach (var menuItem in _viewModel.MainNavigationItems)
                        {
                            if (menuItem is MainNavigationItem menu && menu?.Tag?.Equals(tag) == true)
                            {
                                select = menuItem;
                                break;
                            }
                        }
                    }

                    if (select is null)
                    {
                        foreach (var footerMenuItem in _viewModel.MainNavigationFooterItems)
                        {
                            if (footerMenuItem is MainNavigationItem menu && menu?.Tag?.Equals(tag) == true)
                            {
                                select = footerMenuItem;
                                break;
                            }

                            if (tag == AppPagesEnum.Login.ToString() && footerMenuItem is MainNavigationUserItem)
                            {
                                select = footerMenuItem;
                                break;
                            }

                            if (tag == AppPagesEnum.Settings.ToString() && footerMenuItem is MainNavigationSettingItem)
                            {
                                select = footerMenuItem;
                                break;
                            }
                        }
                    }

                    MainNavigationView.SelectedItem = select;
                }
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void MainFramNavigateToPage(string navItemTag, object parameter = null, Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo = null)
        {
            try
            {
                Type page = null;

                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                page = item.Page;

                var preNavPageType = MainFrame.CurrentSourcePageType;
                if (!(page is null) && !Type.Equals(preNavPageType, page))
                {
                    if (parameter != null || transitionInfo != null)
                    {
                        MainFrame.Navigate(page, parameter, transitionInfo);
                    }
                    else
                    {
                        MainFrame.Navigate(page);
                    }
                }
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        #region 标题栏左上功能按键

        private void OnClickMenu(object sender, RoutedEventArgs e)
        {
            try
            {
                MainNavigationView.IsPaneOpen = !MainNavigationView.IsPaneOpen;
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            TryGoBack();
        }

        private void OnClickRefresh(object sender, RoutedEventArgs e)
        {
            try
            {
                //MainViewModel.RefreshCurrentPage();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"OnClickRefresh Error: {ex.Message}");
            }
        }

        #endregion

        #region 处理应用程序外观

        private void SetTitleBarArea()
        {
            try
            {
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;

                // 设置为可拖动区域
                Window.Current.SetTitleBar(AppTitleBarGrid);

                var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                // 当窗口激活状态改变时，注册一个handler
                Window.Current.Activated += (s, e) =>
                {
                    try
                    {
                        if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
                            AppTitleLogo.Opacity = 0.7;
                        else
                            AppTitleLogo.Opacity = 1.0;
                    }
                    catch (Exception ex) { Trace.WriteLine(ex.Message); }
                };
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void UpdateAppTheme()
        {
            try
            {
                bool isLight = true;
                if (_viewModel.AppSettings.AppearanceIndex == 0)
                {
                    isLight = _themeListener.CurrentTheme != ApplicationTheme.Dark;
                }
                else
                {
                    isLight = _viewModel.AppSettings.AppearanceIndex != 2;
                }

                // 修改标题栏按钮颜色
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                // Set active window colors
                // Note: No effect when app is running on Windows 10 since color customization is not supported.
                titleBar.ForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.ButtonHoverBackgroundColor = isLight ? Windows.UI.Color.FromArgb(10, 0, 0, 0) : Windows.UI.Color.FromArgb(16, 255, 255, 255);
                titleBar.ButtonPressedForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.ButtonPressedBackgroundColor = isLight ? Windows.UI.Color.FromArgb(08, 0, 0, 0) : Windows.UI.Color.FromArgb(10, 255, 255, 255);

                // Set inactive window colors
                // Note: No effect when app is running on Windows 10 since color customization is not supported.
                titleBar.InactiveForegroundColor = Colors.Gray;
                titleBar.InactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                if (Window.Current.Content is FrameworkElement rootElement)
                {
                    if (!isLight)
                    {
                        rootElement.RequestedTheme = ElementTheme.Dark;
                    }
                    else
                    {
                        rootElement.RequestedTheme = ElementTheme.Light;
                    }
                }
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
        }

        private void UpadateAppBackdrop()
        {
            
        }

        #endregion

        #region 返回

        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            // When Alt+Left are pressed navigate back
            if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                && e.VirtualKey == VirtualKey.Left
                && e.KeyStatus.IsMenuKeyDown == true
                && !e.Handled)
            {
                e.Handled = TryGoBack();
            }
        }

        private void System_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = TryGoBack();
            }
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            // Handle mouse back button.
            if (e.CurrentPoint.Properties.IsXButton1Pressed)
            {
                e.Handled = TryGoBack();
            }
        }

        private bool TryGoBack()
        {
            try
            {
                if (!MainFrame.CanGoBack)
                    return false;

                MainFrame.GoBack();
                return true;
            }
            catch (Exception ex) { Trace.WriteLine(ex.Message); }
            return false;
        }

        #endregion

    }
}
