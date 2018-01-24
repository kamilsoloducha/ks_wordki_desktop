using Oazachaosu.Core.Common;
using System.Collections.Generic;
using Wordki.Helpers.WordComparer;
using Wordki.Models;
using Wordki.Models.Lesson;
using WordkiModel;


namespace Wordki.Test.Lessons
{
    public static class LessonUtil
    {

        public static int wordCount = 10;

        public static IEnumerable<IWord> GetWords()
        {
            IGroup group = Utility.GetGroup();
            for (int i = 0; i < wordCount; i++)
            {
                IWord word = new Word()
                {
                    Language1 = $"Language{i}",
                    Language2 = $"Jezyk{i}",
                    Drawer = 3,
                };
                group.AddWord(word);
                yield return word;
            }
        }

        public static IWordComparer GetWordComparer()
        {
            IWordComparer comparer = new WordComparer();
            comparer.Settings = new WordComparerSettings();
            comparer.Settings.WordSeparator = ',';
            comparer.Settings.NotCheckers.Add(new LetterCaseNotCheck());
            comparer.Settings.NotCheckers.Add(new SpaceNotCheck());
            comparer.Settings.NotCheckers.Add(new Utf8NotCheck());
            return comparer;
        }

        public static ILessonSettings GetLessonSettings()
        {
            return new LessonSettings()
            {
                AllWords = true,
                TranslationDirection = TranslationDirection.FromFirst,
            };
        }

    }
}
