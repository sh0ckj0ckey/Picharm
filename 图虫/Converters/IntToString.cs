using System;
using Windows.UI.Xaml.Data;

namespace 图虫.Converters
{
    internal class IntToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value != null)
                {
                    return "共" + value.ToString() + "张";
                }
            }
            catch
            { }
            return "共0张";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
