using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.Lesson
{
    public static class LessonFactory
    {

        public static Lesson CreateLesson(LessonType type)
        {
            Lesson lesson;
            switch (type)
            {
                case LessonType.FiszkiLesson:
                    {
                        lesson = new FiszkiLesson();
                    }
                    break;
                case LessonType.TypingLesson:
                    {
                        lesson = new TypingLesson();
                    }
                    break;
                case LessonType.IntensiveLesson:
                    {
                        lesson = new IntensiveLesson();
                    }
                    break;
                case LessonType.RandomLesson:
                    {
                        lesson = new RandomLesson();
                    }
                    break;
                case LessonType.BestLesson:
                    {
                        lesson = new RandomLesson();
                    }
                    break;
                case LessonType.AllToTeach:
                    {
                        lesson = new TypingLesson();
                    }
                    break;
                default:
                    {
                        throw new IndexOutOfRangeException($"There is not possible to create Lesson for type {type}");
                    }
            }
            return lesson;
        }

    }
}
