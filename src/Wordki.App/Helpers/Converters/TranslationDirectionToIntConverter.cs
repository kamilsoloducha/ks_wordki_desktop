
using Oazachaosu.Core.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class TranslationDirectionToIntConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TranslationDirection))
                return 0;
            TranslationDirection lValue = (TranslationDirection)value;
            return (int)lValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? lValue = value as int?;
            if (lValue != null)
                return (TranslationDirection)lValue;
            else
                return TranslationDirection.FromSecond;
        }
    }
}
