using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace 图虫.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            try
            {
                if (value != null)
                {
                    if (parameter != null && parameter.ToString() == "-")
                    {
                        return value.ToString() == false.ToString() ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else
                    {
                        return value.ToString() == true.ToString() ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("BoolToVisibleConvert " + e1.Message.ToString());
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return null;
        }
    }
}
