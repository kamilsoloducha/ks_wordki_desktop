using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Repository.Models.Enums;

namespace Wordki.Helpers
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

  public class TranslationDirectionToIntConverter : IValueConverter
  {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is TranslationDirection))
        return 0;
      TranslationDirection lValue = (TranslationDirection)value;
      return (int)lValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int? lValue = value as int?;
      if (lValue != null)
        return (TranslationDirection)lValue;
      else
        return TranslationDirection.FromSecond;
    }
  }

  public class MouseButtonEventArgsToPointConverter : IEventArgsConverter
  {
    public object Convert(object value, object parameter)
    {
      var args = (MouseEventArgs)value;
      var element = (FrameworkElement)parameter;
      var point = args.GetPosition(element);
      return point;
    }
  }
}
