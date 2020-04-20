using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace 图虫.Converters
{
    internal class StringLengthToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var bReverseResult = parameter?.ToString() == "-";
                var bVisible = value is string content && !string.IsNullOrEmpty(content);
                if (bReverseResult)
                {
                    bVisible = !bVisible;
                }

                return bVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

}
