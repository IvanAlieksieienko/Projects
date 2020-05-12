using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Finder.UI.Additional.Converters
{
    public class SearchTypeToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var searchTypeInt = (int)value;
            Thickness result = searchTypeInt == 0 ? new Thickness(0, 125, 0, 0) : searchTypeInt == 1 ? new Thickness(0, 257, 0, 35) : new Thickness(0, 125, 0, 0);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var height = (int)value;
            return height;
        }
    }
}
