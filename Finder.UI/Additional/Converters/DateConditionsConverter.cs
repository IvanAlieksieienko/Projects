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
    public class DateConditionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumArray = ((ObservableCollection<DateCondition>)value).ToArray();
            ObservableCollection<String> result = new ObservableCollection<string>();
            Array.ForEach(enumArray, (enumValue) =>
            {
                var item = enumValue == DateCondition.LaterThan ? "Later than" : enumValue == DateCondition.EarlierThan ?
                "Earlier than" : enumValue == DateCondition.InThatDay ? "In that day" : "Later than";
                result.Add(item);
            });
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringArray = ((ObservableCollection<String>)value).ToArray();
            ObservableCollection<DateCondition> enumArray = new ObservableCollection<DateCondition>();
            Array.ForEach(stringArray, (String stringValue) =>
            {
                var str = (String)stringValue;
                var result = str == "Later than" ? DateCondition.LaterThan :
                    str == "Earlier than" ? DateCondition.EarlierThan :
                    str == "In that day" ? DateCondition.InThatDay : DateCondition.EarlierThan;
                enumArray.Add(result);
            });
            return enumArray;
        }
    }

    public class DateConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (DateCondition)value;
            var result = enumValue == DateCondition.LaterThan ? "Later than" : enumValue == DateCondition.EarlierThan ?
                "Earlier than" : enumValue == DateCondition.InThatDay ? "In that day" : "Later than";
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var stringValue = (String)value;
            var result = stringValue == "Later than" ? DateCondition.LaterThan :
                stringValue == "Earlier than" ? DateCondition.EarlierThan :
                stringValue == "In that day" ? DateCondition.InThatDay : DateCondition.EarlierThan;
            return result;
        }
    }
}
