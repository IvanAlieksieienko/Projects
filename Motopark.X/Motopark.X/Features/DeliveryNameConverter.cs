using Motopark.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Motopark.X.Features
{
    public class DeliveryNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            var str = ((DeliveryType)value).ToString();
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DeliveryType.Department;
            if (value != null)
            {
                var str = (string)value;
                result = str == DeliveryType.Department.ToString() ? DeliveryType.Department :
                    str == DeliveryType.Courier.ToString() ? DeliveryType.Department : DeliveryType.Courier;
            }
            return result;
        }
    }
}
