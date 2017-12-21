using WordkiModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Wordki.Database;
using Wordki.Models.LessonScheduler;

namespace Wordki.Helpers.Converters
{
    public class GroupToNextRepeatConverter : IValueConverter
    {

        private static ILessonScheduler scheduler;
        private static ILessonScheduleInitializer schedulerInitializer;

        static GroupToNextRepeatConverter()
        {
            schedulerInitializer = new LessonSchedulerInitializer2(new List<int>() { 1, 1, 2, 4, 7 });
            scheduler = new NewLessonScheduler()
            {
                Initializer = schedulerInitializer,
            };
        }

        //public GroupToNextRepeatConverter()
        //{

        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IGroup group = value as IGroup;
            if(group == null)
            {
                return 0;
            }
            schedulerInitializer.TranslationDirection = UserManagerSingleton.Instence.User.TranslationDirection;
            return Math.Max(scheduler.GetTimeToLearn(group), 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
