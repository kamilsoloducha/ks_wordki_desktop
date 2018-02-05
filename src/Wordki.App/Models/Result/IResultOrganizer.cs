using System;

namespace Wordki.Models
{
    public interface IResultOrganizer
    {
        int GetLessonTime(DateTime start, DateTime end);

        int GetLessonTimeToday();
    }
}