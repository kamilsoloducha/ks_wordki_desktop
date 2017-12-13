using Repository.Models;
using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Wordki.Helpers.Converters
{
    public class GroupsToResultsCountMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<IGroup> groups = values[0] as IEnumerable<IGroup>;
            if(groups == null)
            {
                return 0;
            }
            TranslationDirection direction;
            try
            {
                direction = (TranslationDirection)values[1];
            }catch(InvalidCastException)
            {
                direction = TranslationDirection.FromFirst;
                Console.WriteLine($"Error during cast translationDirection in GroupsToResultsCountMultiConverter." +
                    $"Casted value: {values[1]}");
            }
            return groups.Sum(x => x.Results.Where(y => y.TranslationDirection == direction).Count());

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
