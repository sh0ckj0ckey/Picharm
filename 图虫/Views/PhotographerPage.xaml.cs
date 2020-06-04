using MaterialLibs.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 图虫.Helpers;
using 图虫.Models.Photograph;
using 图虫.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PhotographerPage : Page, INotifyPropertyChanged
    {
        private string _photographerID;
        private int _page = 2;
        private string _beforetimestamp = "0";
        private bool shouldGoBack = false;
        ObservableCollection<AllPhotographViewModel> PhotographObservableCollection = new ObservableCollection<AllPhotographViewModel>();
        ObservableCollection<FeedViewModel> PhotographViewModelsObservableCollection = new ObservableCollection<FeedViewModel>();

        bool _emergencyStop = false;

        private PhotographerViewModel _viewModel;
        PhotographerViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public PhotographerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter.GetType().Equals(typeof(bool)))
                {
                    shouldGoBack = (bool)e.Parameter;
                }
                try
                {
                    _emergencyStop = false;
                    _photographerID = MsgBus.Instance.PhotographerID.Substring(4);
                    LoadInfo(_photographerID);
                    LoadPhotos(_photographerID);
                }
                catch
                {
                    FailedGrid.Visibility = Visibility.Visible;
                }
            }
            catch { }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                _emergencyStop = true;
                PhotographObservableCollection?.Clear();
                PhotographViewModelsObservableCollection?.Clear();
                GC.Collect();
            }
            catch { }
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// 加载个人资料
        /// </summary>
        /// <param name="id"></param>
        private async void LoadInfo(string id)
        {
            try
            {
                var info = await TuchongApi.GetPhotographerInfo(id);
                if (info == null)
                {
                    return;
                }
                ViewModel = new PhotographerViewModel(info);
            }
            catch
            {
                FailedGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 初始化获取第一页的作品
        /// </summary>
        /// <param name="id"></param>
        private async void LoadPhotos(string id)
        {
            try
            {
                string para = "count=20&page=1";
                Photograph photographs = await TuchongApi.GetPhotograph(para, id);
                if (photographs == null || photographs.result == "ERROR" || photographs.result == "FAILED")
                {
                    FailedGrid.Visibility = Visibility.Visible;
                    return;
                }
                foreach (var item in photographs.post_list)
                {
                    if (_emergencyStop)
                    {
                        break;
                    }
                    try
                    {
                        var listItem = new AllPhotographViewModel { Cover = item.images[0].source.l, Count = item.image_count };
                        await listItem.LoadImageAsync();
                        PhotographObservableCollection.Add(listItem);
                        PhotographViewModelsObservableCollection.Add(new FeedViewModel(new PhotographViewModel(item)));
                    }
                    catch { continue; }
                }
                if (photographs.more == false)
                {
                    LoadmoreButton.Visibility = Visibility.Collapsed;
                    EndImage.Visibility = Visibility.Visible;
                }
                else
                {
                    LoadmoreButton.Visibility = Visibility.Visible;
                    EndImage.Visibility = Visibility.Collapsed;
                }
                _beforetimestamp = photographs.before_timestamp;
            }
            catch (Exception e)
            {
                DialogShower.ShowDialog("应用程序遇到了异常", e.Message);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FailedGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MsgBus.Instance.PhotographerID = "ID: " + _photographerID;
            this.Frame.Navigate(typeof(PhotographerPage), false);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack && shouldGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {
                this.Frame.Navigate(typeof(BlankPage));
            }
        }

        /// <summary>
        /// 鼠标指向某一张图片的动画效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement s)
            {
                VisualHelper.SetScale(s, "1.1,1.1,1.1");
            }
        }

        /// <summary>
        /// 鼠标指向某一张图片的动画效果
        /// </summary9>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement s)
            {
                VisualHelper.SetScale(s, "1,1,1");
            }
        }

        /// <summary>
        /// 查看作品详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicturesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (PicturesListView.SelectedIndex < 0)
                {
                    return;
                }
                // 第二个参数 true 表示这个 MorePicturesPage 是从摄影师页面跳转的，返回的时候不要直接回到 BlankPage
                this.Frame.Navigate(typeof(MorePicturesPage), PhotographViewModelsObservableCollection[PicturesListView.SelectedIndex]);
            }
            catch { }
        }

        /// <summary>
        /// 点击加载更多作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string para = "count=20&page=" + _page + "&before_timestamp=" + _beforetimestamp;
                Photograph photographs = await TuchongApi.GetPhotograph(para, _photographerID);
                if (photographs == null || photographs.result == "ERROR" || photographs.result == "FAILED")
                {
                    FailedGrid.Visibility = Visibility.Visible;
                    return;
                }
                foreach (var item in photographs.post_list)
                {
                    if (_emergencyStop)
                    {
                        break;
                    }
                    try
                    {
                        var listItem = new AllPhotographViewModel { Cover = item.images[0].source.m, Count = item.image_count };
                        await listItem.LoadImageAsync();
                        PhotographObservableCollection.Add(listItem);
                        PhotographViewModelsObservableCollection.Add(new FeedViewModel(new PhotographViewModel(item)));
                    }
                    catch
                    {
                        continue;
                    }
                }
                if (photographs.more == false)
                {
                    LoadmoreButton.Visibility = Visibility.Collapsed;
                    EndImage.Visibility = Visibility.Visible;
                }
                else
                {
                    LoadmoreButton.Visibility = Visibility.Visible;
                    EndImage.Visibility = Visibility.Collapsed;
                }
                _page++;
                _beforetimestamp = photographs.before_timestamp;
            }
            catch { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///  关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoginHelper.LoggedIn == false)
                {
                    DialogShower.ShowDialog("请先登录哦");
                    return;
                }
                var resp = await TuchongApi.PutFollow(ViewModel.ID);
                if (resp != null)
                {
                    if (resp.result == "SUCCESS")
                    {
                        ViewModel.isFollowing = true;
                    }
                    else
                    {
                        DialogShower.ShowDialog(resp.message);
                    }
                }
                else
                {
                    DialogShower.ShowDialog("关注失败，数据解析错误");
                }
            }
            catch (Exception ex)
            {
                DialogShower.ShowDialog("关注异常", ex.Message);
            }
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoginHelper.LoggedIn == false)
                {
                    DialogShower.ShowDialog("请先登录哦");
                    return;
                }
                var resp = await TuchongApi.DeleteFollow(ViewModel.ID);
                if (resp != null)
                {
                    if (resp.result == "SUCCESS")
                    {
                        ViewModel.isFollowing = false;
                    }
                    else
                    {
                        DialogShower.ShowDialog(resp.message);
                    }
                }
                else
                {
                    DialogShower.ShowDialog("取消关注失败，数据解析错误");
                }
            }
            catch (Exception ex)
            {
                DialogShower.ShowDialog("取关异常", ex.Message);
            }
        }
    }

    public class AllPhotographViewModel
    {
        public string Cover { get; set; }
        public double Count { get; set; } = 1;

        public BitmapImage ImageSource { get; set; }

        public async Task LoadImageAsync(int decodeWidth = 196)
        {
            ImageSource = await ImageLoader.LoadImageAsync(Cover);
            ImageSource.DecodePixelType = DecodePixelType.Logical;
            ImageSource.DecodePixelWidth = decodeWidth;
        }
    }


}
