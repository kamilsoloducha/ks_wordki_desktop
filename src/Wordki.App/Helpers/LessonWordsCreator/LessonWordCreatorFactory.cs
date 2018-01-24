using WordkiModel.Enums;
using System;

namespace Wordki.Helpers
{
    public static class LessonWordCreatorFactory
    {

        public static ILessonWordsCreator Create(LessonType type)
        {
            ILessonWordsCreator result;
            switch (type)
            {
                case LessonType.FiszkiLesson:
                    {
                        result = new SimpleCreator();
                    }
                    break;
                case LessonType.TypingLesson:
                    {
                        result = new SimpleCreator();
                    }
                    break;
                case LessonType.IntensiveLesson:
                    {
                        result = new SimpleCreator();
                    }
                    break;
                case LessonType.RandomLesson:
                    {
                        result = new RandomCreator();
                    }
                    break;
                case LessonType.BestLesson:
                    {
                        result = new BestCreator();
                    }
                    break;
                case LessonType.AllToTeach:
                    {
                        result = new AllToTeachCreator();
                    }
                    break;
                default:
                    {
                        throw new IndexOutOfRangeException($"There is not possible to create Lesson for type {type}");
                    }
            }
            return result;
        }

    }
}
