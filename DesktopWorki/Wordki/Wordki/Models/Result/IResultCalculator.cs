using System;
using System.Collections.Generic;
using Repository.Models;

namespace Wordki.Models
{
    public interface IResultCalculator
    {
        int GetLessonTime(DateTime start, DateTime end);
    }
}