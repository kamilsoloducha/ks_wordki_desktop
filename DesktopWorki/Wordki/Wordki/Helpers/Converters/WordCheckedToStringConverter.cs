using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class WordCheckedToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? wordChecked = value as bool?;
            if (!wordChecked.HasValue)
            {
                return "%ERROR%";
            }
            return wordChecked.Value ? "Odznacz" : "Zaznacz";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
