using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 图虫.Helpers;
using 图虫.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImageDetailPage : Page
    {
        bool DarkMode = false;
        FeedViewModel oneFeed;
        int PageIndex = 0;
        float zoomFactor = 1;
        double pressX = 0;
        double pressY = 0;
        ObservableCollection<ImageExifViewModel> imageExifViewModels = new ObservableCollection<ImageExifViewModel>();
        ObservableCollection<AsyncBitmapImage> allPictures = new ObservableCollection<AsyncBitmapImage>();

        bool isCtrlPressed = false;

        bool _emergencyStop = false;

        int _loadedCount = 0;
        public ImageDetailPage()
        {
            this.InitializeComponent();
            DetailPage.Focus(FocusState.Keyboard);
            try
            {
                DetailPage.KeyDown += (sender, e) =>
                {
                    isCtrlPressed = (e.Key == VirtualKey.Control);
                };
                DetailPage.KeyUp += (sender, e) =>
                {
                    if (e.Key == VirtualKey.Control)
                    {
                        isCtrlPressed = false;
                    }
                };
            }
            catch
            {
                DialogShower.ShowDialog("注册按键监听失败，Ctrl相关快捷键将失效，重启应用可能会解决。");
            }
            if (MainPage.SettingContainer.Values["knowthetip"] == null || MainPage.SettingContainer.Values["knowthetip"].ToString() == "false")
            {
                TipStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TipStackPanel.Visibility = Visibility.Collapsed;
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
                        // 订阅按下和抬起按键的事件
                        Window.Current.CoreWindow.KeyDown += CoreWindowKeyDown;
                        Window.Current.CoreWindow.KeyUp += CoreWindowKeyUp;
                    }
                    catch
                    {
                        DialogShower.ShowDialog("注册按键监听失败，Ctrl相关快捷键将失效，重启应用可能会解决。");
                    }
                    oneFeed = (FeedViewModel)e.Parameter;
                    _emergencyStop = false;
                    _loadedCount = 0;
                    LoadPictures();
                }
                else
                {
                    this.Frame.GoBack();
                }
            }
            catch { }
        }

        // 离开当前页面前取消订阅事件
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                _emergencyStop = true;
                try
                {
                    Window.Current.CoreWindow.KeyDown -= CoreWindowKeyDown;
                }
                catch { }
                try
                {
                    Window.Current.CoreWindow.KeyUp -= CoreWindowKeyUp;
                }
                catch { }

                // 把可能开启夜间模式后导致的右上角三个键改回黑色字体
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonForegroundColor = Colors.Black;

                allPictures.Clear();
                GC.Collect();
            }
            catch { }
        }

        private async void LoadPictures()
        {
            try
            {
                LoadingProgressRing.IsActive = true;
                LoadingGrid.Visibility = Visibility.Visible;
                foreach (var item in oneFeed.Pictures)
                {
                    if (_emergencyStop)
                    {
                        break;
                    }
                    await item.LoadImageAsync(96);
                    allPictures.Add(item);
                    _loadedCount++;
                    LoadingTextBlock.Text = "正在加载缩略图(" + _loadedCount + "/" + oneFeed.PicsCount + ")";
                }
                if (oneFeed.ReadingIndex >= 0 && CarouselControl.SelectedIndex < 0)
                {
                    CarouselControl.SelectedIndex = oneFeed.ReadingIndex;
                }
                LoadingProgressRing.IsActive = false;
                LoadingGrid.Visibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                DialogShower.ShowDialog("缩略图加载失败", e.Message);
            }
        }

        // 按下Ctrl时设置isCtrlPressed为true
        private void CoreWindowKeyDown(Windows.UI.Core.CoreWindow window, Windows.UI.Core.KeyEventArgs args)
        {
            isCtrlPressed = (args.VirtualKey == VirtualKey.Control);
        }

        // 抬起Ctrl设置为false
        private void CoreWindowKeyUp(Windows.UI.Core.CoreWindow window, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Control)
            {
                isCtrlPressed = false;
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        /// <summary>
        /// 开关灯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (DarkMode == false)
            {
                DarkMode = true;
                DetailPage.RequestedTheme = ElementTheme.Dark;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonForegroundColor = Colors.White;
                ThemeTextBlock.Text = "\uEB4F";
            }
            else
            {
                DarkMode = false;
                DetailPage.RequestedTheme = ElementTheme.Light;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonForegroundColor = Colors.Black;
                ThemeTextBlock.Text = "\uEA80";
            }
        }

        /// <summary>
        /// 查看图片参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.Visibility = Visibility.Visible;
            DataGridPopIn.Begin();
            TitleTextBlock.Text = "图片编号：" + oneFeed.PicturesID[PageIndex];
            try
            {
                imageExifViewModels = await TuchongApi.GetImageExif(oneFeed.PicturesID[0]);
                ExifListView.ItemsSource = imageExifViewModels;
            }
            catch { }
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataGridPopOut.Begin();
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            DataGridPopOut.Begin();
        }

        private void DataGridPopOut_Completed(object sender, object e)
        {
            DataGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 鼠标拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainImageScroller_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(MainImageScroller).Properties.IsLeftButtonPressed)
            {
                var p = e.GetCurrentPoint(MainImageScroller).Position;
                MainImageScroller.ChangeView(MainImageScroller.HorizontalOffset - p.X + pressX, MainImageScroller.VerticalOffset - p.Y + pressY, null, true);
                pressX = p.X;
                pressY = p.Y;
            }
        }

        /// <summary>
        /// 双击放大和复原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainImageScroller_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (MainImageScroller.ZoomFactor - zoomFactor > 0.1 || MainImageScroller.ZoomFactor - zoomFactor < -0.1)
            {
                MainImageScroller.ChangeView(0, 0, zoomFactor);
            }
            else
            {
                var p = e.GetPosition(DetailImage);
                MainImageScroller.ChangeView(p.X, p.Y, zoomFactor + 0.5f);
            }
        }

        /// <summary>
        /// 按下鼠标准备拖拽的时候先记录当前的坐标，后面 Move 时用来计算拖拽的偏移量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainImageScroller_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pressX = e.GetCurrentPoint(MainImageScroller).Position.X;
            pressY = e.GetCurrentPoint(MainImageScroller).Position.Y;
        }

        /// <summary>
        /// 切换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllPicturesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartSwitchImage.Begin();
        }

        private void DoubleAnimation_Completed(object sender, object e)
        {
            ImageTitleTextBlock.Text = oneFeed.PicturesTitle[CarouselControl.SelectedIndex];
            // await oneFeed.Pictures[CarouselControl.SelectedIndex].LoadRawImageAsync();
            DetailImage.Source = new BitmapImage(new Uri(oneFeed.Pictures[CarouselControl.SelectedIndex].ImageUri));
            EndSwitchImage.Begin();
        }

        /// <summary>
        /// 将图片缩放居中
        /// </summary>
        private void CenterPicture()
        {
            zoomFactor = (float)Math.Min(MainImageScroller.ActualWidth / DetailImage.ActualWidth,
                MainImageScroller.ActualHeight / DetailImage.ActualHeight);
            MainImageScroller.ChangeView(null, null, zoomFactor);
        }

        /// <summary>
        /// 切换图片后，重新加载完成图片后将图片居中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            CenterPicture();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainPage.SettingContainer.Values["knowthetip"] = "true";
            TipStackPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 滚轮切换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailImage_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (isCtrlPressed)
                {
                    return;
                }
                // 图片不在放大状态时，滚轮切换上下页
                if (MainImageScroller.ZoomFactor - zoomFactor < 0.1)
                {
                    int wheel = e.GetCurrentPoint(DetailImage).Properties.MouseWheelDelta;
                    if (wheel > 0)
                    {
                        if (CarouselControl.SelectedIndex > 0)
                        {
                            CarouselControl.SelectedIndex--;
                        }
                    }
                    else if (wheel < 0)
                    {
                        if (CarouselControl.SelectedIndex < oneFeed.Pictures.Count - 1)
                        {
                            CarouselControl.SelectedIndex++;
                        }
                    }
                }
            }
            catch { }
        }
    }
}
