using WordkiModel;
using System;
using System.Globalization;
using System.Windows.Data;
using WordkiModel.Enums;

namespace Wordki.Helpers.Converters
{
    public class TranslationDirectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TranslationDirection direction = (TranslationDirection)value;
            return direction == TranslationDirection.FromFirst ? "Z pierwszego" : "Z drugiego";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
