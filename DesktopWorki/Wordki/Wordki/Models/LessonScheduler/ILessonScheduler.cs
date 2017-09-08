using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.LessonScheduler
{
    public interface ILessonScheduler
    {

        int GetTimeToLearn(ICollection<Result> results);
        int GetColor(Result resut);

    }
}
