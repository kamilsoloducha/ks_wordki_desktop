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
            IUser user = value as IUser;
            if(user == null)
            {
                throw new ArgumentException("The value parameter is not IUser object");
            }
            return user.TranslationDirection == Repository.Models.Enums.TranslationDirection.FromFirst ? "Z pierwszego" : "Z drugiego";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
