using Oazachaosu.Core.Common;
using System;
using System.Globalization;
using System.Windows.Data;
using WordkiModel;

namespace Wordki.Helpers.Converters
{
    public class LanguageTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LanguageType language = (LanguageType)value;
            return LanguageFactory.GetLanguage(language).Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
