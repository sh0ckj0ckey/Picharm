using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using 图虫.Helpers;

namespace 图虫.ViewModels
{
    public class HoteventViewModel
    {
        public string Title { get; set; }
        public string ImageCount { get; set; }
        public string DueDays { get; set; }
        public string CoverImage { get; set; }
        public string PrizeDesc { get; set; }
        public string Url { get; set; }
        public BitmapImage ImageSource { get; set; }

        public async Task LoadImageAsync()
        {
            ImageSource = await ImageLoader.LoadImageAsync(CoverImage);
            ImageSource.DecodePixelType = DecodePixelType.Logical;
            ImageSource.DecodePixelHeight = 224;
        }

        public HoteventViewModel(Models.Discover.Hotevent hotevent)
        {
            if (hotevent.title.Length <= 26)
            {
                this.Title = hotevent.title;
            }
            else
            {
                this.Title = hotevent.title.Substring(0, 24) + "...";
            }
            this.ImageCount = hotevent.image_count + "件作品";
            this.CoverImage = hotevent.images[0];
            this.PrizeDesc = hotevent.prize_desc;
            this.DueDays = "距截稿" + hotevent.dueDays + "天";
            this.Url = hotevent.url;
        }
    }
}
