using Repository.Models.Enums;
using System;

namespace Wordki.Models.Lesson
{
    [Serializable]
    public class LessonSettings : ILessonSettings
    {
        public bool AllWords { get; set; }
        public TranslationDirection TranslationDirection { get; set; }
        public int Timeout { get; set; }

        public override bool Equals(object obj)
        {
            LessonSettings lessonSettings = obj as LessonSettings;
            return lessonSettings != null
                && lessonSettings.AllWords == AllWords
                && lessonSettings.TranslationDirection == TranslationDirection
                && lessonSettings.Timeout == Timeout;
        }

    }
}
