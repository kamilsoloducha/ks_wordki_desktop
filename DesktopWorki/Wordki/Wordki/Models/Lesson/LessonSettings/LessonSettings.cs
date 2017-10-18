using Repository.Models;
using Repository.Models.Enums;

namespace Wordki.Models.Lesson
{
    public class LessonSettings : ILessonSettings
    {
        public bool AllWords { get; set; }
        public TranslationDirection TranslationDirection { get; set; }
        public int Timeout { get; set; }

    }
}
