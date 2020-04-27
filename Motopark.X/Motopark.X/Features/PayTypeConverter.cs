using Motopark.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Motopark.X.Features
{
    public class PayTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ((PaymentType)value).ToString();
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = PaymentType.Cash;
            if (value != null)
            {
                var str = (string)value;
                result = str == PaymentType.Cash.ToString() ? PaymentType.Cash :
                    str == PaymentType.Cashless.ToString() ? PaymentType.Cash : PaymentType.Cashless;
            }
            return result;
        }
    }
}
