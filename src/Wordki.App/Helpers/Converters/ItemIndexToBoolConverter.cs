using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class ItemIndexToBoolConverter : IMultiValueConverter
    {
        public CheckingElement Index { get; set; }

        private IList collection;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
            {
                return false;
            }
            if (collection == null)
            {
                collection = values[1] as IList;
            }
            if (collection.Count < 2)
            {
                return false;
            }
            bool result = collection.IndexOf(values[0]) != GetValueToCompare();
            return result;
        }

        private int GetValueToCompare()
        {
            if (Index == CheckingElement.First)
            {
                return 0;
            }
            else
            {
                return collection.Count - 1;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public enum CheckingElement
    {
        First,
        Last,
    }
}
