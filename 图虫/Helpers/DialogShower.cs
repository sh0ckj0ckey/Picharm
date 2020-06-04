using System;
using Windows.UI.Xaml.Controls;

namespace 图虫.Helpers
{
    public static class DialogShower
    {
        public static async void ShowDialog(string content)
        {
            var dialog = new ContentDialog()
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

        public static async void ShowDialog(string title, string content)
        {
            var dialog = new ContentDialog()
            {
                Title = title,
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
