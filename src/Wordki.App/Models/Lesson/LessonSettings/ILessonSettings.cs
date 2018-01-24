

using Oazachaosu.Core.Common;

namespace Wordki.Models.Lesson
{

    public interface ILessonSettings
    {

        bool AllWords { get; set; }
        TranslationDirection TranslationDirection { get; set; }
        int Timeout { get; set; }

    }
}
