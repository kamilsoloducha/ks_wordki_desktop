using System;
using System.Globalization;
using System.Windows.Data;
using Repository.Models.Language;

namespace Wordki.Helpers.Converters
{
    public class LanguageToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return LanguageFactory.GetLanguage((LanguageType)value).Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
