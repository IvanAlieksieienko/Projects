using Motopark.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Motopark.X.Features
{
    public class PostNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ((PostType)value).ToString();
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = PostType.NovaPoshta;
            if (value != null)
            {
                var str = (string)value;
                result = str == PostType.Self.ToString() ? PostType.Self : 
                    str == PostType.NovaPoshta.ToString() ? PostType.NovaPoshta : 
                    str == PostType.MeestExpress.ToString() ? PostType.MeestExpress : 
                    str == PostType.UkrPoshta.ToString() ? PostType.UkrPoshta : PostType.NovaPoshta;
            }
            return result;
        }
    }
}
