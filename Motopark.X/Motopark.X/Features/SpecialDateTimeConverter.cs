using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Motopark.X.Features
{
    public class SpecialDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("d-MM-yyyy HH:mm:ss");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("d-MM-yyyy HH:mm:ss");
        }
    }
}
