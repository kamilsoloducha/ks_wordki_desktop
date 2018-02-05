using WordkiModel;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class WordToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IWord word = value as IWord;
            if(word == null)
            {
                return null;
            }
            return word.Group.Words.IndexOf(word) + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
