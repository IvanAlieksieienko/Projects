using Finder.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Finder.UI.Additional.Converters
{
    public class SizeConditionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumArray = ((ObservableCollection<SizeConditionEnum>)value).ToArray();
            ObservableCollection<String> result = new ObservableCollection<string>();
            Array.ForEach(enumArray, (enumValue) =>
            {
                var item = enumValue == SizeConditionEnum.BiggerThan ? "Bigger than" : enumValue == SizeConditionEnum.SmallerThan ?
                "Smaller than" : enumValue == SizeConditionEnum.Equal ? "Equal" : "Bigger than";
                result.Add(item);
            });
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringArray = ((ObservableCollection<String>)value).ToArray();
            ObservableCollection<SizeConditionEnum> enumArray = new ObservableCollection<SizeConditionEnum>();
            Array.ForEach(stringArray, (String stringValue) =>
            {
                var str = (String)stringValue;
                var result = str == "Bigger than" ? SizeConditionEnum.BiggerThan :
                    str == "Smaller than" ? SizeConditionEnum.SmallerThan :
                    str == "Equal" ? SizeConditionEnum.Equal : SizeConditionEnum.SmallerThan;
                enumArray.Add(result);
            });
            return enumArray;
        }
    }

    public class SizeConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (SizeConditionEnum)value;
            var result = enumValue == SizeConditionEnum.BiggerThan ? "Bigger than" : enumValue == SizeConditionEnum.SmallerThan ?
                "Smaller than" : enumValue == SizeConditionEnum.Equal ? "Equal" : "Bigger than";
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = (String)value;
            var result = stringValue == "Bigger than" ? SizeConditionEnum.BiggerThan :
                    stringValue == "Smaller than" ? SizeConditionEnum.SmallerThan :
                    stringValue == "Equal" ? SizeConditionEnum.Equal : SizeConditionEnum.SmallerThan;
            return result;
        }
    }
}
