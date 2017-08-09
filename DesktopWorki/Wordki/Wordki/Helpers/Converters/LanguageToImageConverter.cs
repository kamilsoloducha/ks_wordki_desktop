using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Repository.Models.Language;
using Wordki.Models;

namespace Wordki.Helpers.Converters {
  public class LanguageToImageConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      ILanguage language = value as ILanguage;
      string path = language == null ?
        LanguageIconManager.GetPathCircleFlag(LanguageFactory.GetLanguage(LanguageType.Default)) :
        LanguageIconManager.GetPathCircleFlag(language);
      return new BitmapImage(new Uri(path));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
