using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace 图虫.Converters
{
    public class IntToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            try
            {
                if (value != null)
                {
                    if (parameter == null)
                    {
                        return int.Parse(value.ToString()) > 0 ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else if (parameter.ToString() == "-")
                    {
                        return int.Parse(value.ToString()) > 0 ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
            }
            catch { }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return null;
        }
    }
}
