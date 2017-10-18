using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.Lesson
{
    public interface ILessonSettings
    {

        bool AllWords { get; set; }
        TranslationDirection TranslationDirection { get; set; }
        int Timeout { get; set; }

    }
}
