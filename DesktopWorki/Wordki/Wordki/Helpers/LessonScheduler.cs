using System;
using System.Collections.Generic;
using System.Linq;
using Wordki.Models;

namespace Wordki.Helpers {
  public class LessonScheduler {

    public static int TimeToLearn(ICollection<Result> results) {
      return results.Any() ? GetTime(results.Count(), GetIntervalInDays(results.Last())) : 0;
    }

    public static int GetTime(int pCount, int pDifference) {
      int lTime;
      if (pCount > 0 && pCount <= 2) {
        lTime = 1 - pDifference;
      } else if (pCount > 2 && pCount <= 4) {
        lTime = 2 - pDifference;
      } else if (pCount > 4 && pCount <= 6) {
        lTime = 3 - pDifference;
      } else if (pCount > 4 && pCount <= 6) {
        lTime = 4 - pDifference;
      } else {
        lTime = 5 - pDifference;
      }
      return lTime < 0 ? 0 : lTime;
    }

    public static int GetIntervalInDays(Result lResult) {
      DateTime lEndDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
      return (lEndDay - lResult.DateTime).Days;
    }

    public static int GetColor(Result lResult) {
      int lDays = lResult == null ? 8 : GetIntervalInDays(lResult);
      if (lDays < 2) {
        return 4;
      }
      if (lDays < 4) {
        return 3;
      }
      if (lDays < 6) {
        return 2;
      }
      if (lDays < 8) {
        return 1;
      }
      return 0;
    }
  }
}
