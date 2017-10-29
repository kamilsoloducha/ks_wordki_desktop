using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Wordki.Helpers.Converters
{
    public class WordCheckedToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            bool? wordChecked = value as bool?;
            if (!wordChecked.HasValue)
            {
                return App.Current.Resources["UsedNormalFrontBrush"];
            }
            return wordChecked.Value ? Brushes.Red : App.Current.Resources["UsedNormalFrontBrush"];

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
