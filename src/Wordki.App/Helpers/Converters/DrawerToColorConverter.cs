using System;
using System.Globalization;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class DrawerToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int?))
                return Constants.ColorArray[0];
            int? lValue = value as int?;
            if (lValue == 4)
                return Constants.ColorArray[4];
            if (lValue == 3)
                return Constants.ColorArray[3];
            if (lValue == 2)
                return Constants.ColorArray[2];
            if (lValue == 1)
                return Constants.ColorArray[1];

            return Constants.ColorArray[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
