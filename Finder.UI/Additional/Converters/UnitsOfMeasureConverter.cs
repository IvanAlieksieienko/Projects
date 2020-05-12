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
    public class UnitsOfMeasureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumArray = ((ObservableCollection<UnitOfMeasure>)value).ToArray();
            ObservableCollection<String> result = new ObservableCollection<string>();
            Array.ForEach(enumArray, (enumValue) =>
            {
                var item = enumValue.ToString();
                result.Add(item);
            });
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringArray = ((ObservableCollection<String>)value).ToArray();
            ObservableCollection<UnitOfMeasure> enumArray = new ObservableCollection<UnitOfMeasure>();
            Array.ForEach(stringArray, (String stringValue) =>
            {
                var str = (String)stringValue;
                var result = str == UnitOfMeasure.Bit.ToString() ? UnitOfMeasure.Bit :
                    str == UnitOfMeasure.Byte.ToString() ? UnitOfMeasure.Byte :
                    str == UnitOfMeasure.KiloByte.ToString() ? UnitOfMeasure.KiloByte :
                    str == UnitOfMeasure.MegaByte.ToString() ? UnitOfMeasure.MegaByte :
                    str == UnitOfMeasure.GigaByte.ToString() ? UnitOfMeasure.GigaByte :
                    str == UnitOfMeasure.TeraByte.ToString() ? UnitOfMeasure.TeraByte : UnitOfMeasure.Byte;
                enumArray.Add(result);
            });
            return enumArray;
        }
    }

    public class UnitOfMeasureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (UnitOfMeasure)value;
            return enumValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (String)value;
            var result = str == UnitOfMeasure.Bit.ToString() ? UnitOfMeasure.Bit :
                str == UnitOfMeasure.Byte.ToString() ? UnitOfMeasure.Byte :
                str == UnitOfMeasure.KiloByte.ToString() ? UnitOfMeasure.KiloByte :
                str == UnitOfMeasure.MegaByte.ToString() ? UnitOfMeasure.MegaByte :
                str == UnitOfMeasure.GigaByte.ToString() ? UnitOfMeasure.GigaByte :
                str == UnitOfMeasure.TeraByte.ToString() ? UnitOfMeasure.TeraByte : UnitOfMeasure.Byte;
            return result;
        }
    }
}
