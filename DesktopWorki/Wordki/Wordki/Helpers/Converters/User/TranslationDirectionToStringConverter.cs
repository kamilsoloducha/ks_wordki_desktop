using Repository.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class TranslationDirectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Repository.Models.Enums.TranslationDirection direction = (Repository.Models.Enums.TranslationDirection)value;
            return direction == Repository.Models.Enums.TranslationDirection.FromFirst ? "Z pierwszego" : "Z drugiego";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
