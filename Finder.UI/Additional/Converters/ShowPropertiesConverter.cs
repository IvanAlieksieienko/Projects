﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Finder.UI.Additional.Converters
{
    public class ShowPropertiesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isStarted = (bool)value;
            var result = isStarted == true ? Visibility.Hidden : Visibility.Visible;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var height = (int)value;
            return height;
        }
    }
}
