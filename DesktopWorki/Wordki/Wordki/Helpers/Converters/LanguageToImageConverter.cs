using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Helpers.Converters
{
    public class LanguageToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LanguageType language = (LanguageType)value;
            return new BitmapImage(new Uri(LanguageIconManager.GetPathCircleFlag(LanguageFactory.GetLanguage(language))));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
