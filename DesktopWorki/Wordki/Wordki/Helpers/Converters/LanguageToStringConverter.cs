using System;
using System.Globalization;
using System.Windows.Data;
using Repository.Models.Language;

namespace Wordki.Helpers.Converters {
  public class LanguageToStringConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      ILanguage language = value as ILanguage;
      return language == null ? LanguageFactory.GetLanguage(LanguageType.Default).Description : language.Description;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
