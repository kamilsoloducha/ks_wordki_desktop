using WordkiModel;

namespace Wordki.Models.LessonScheduler
{
    public interface ILessonScheduler
    {

        int GetTimeToLearn(IGroup group);
        int GetColor(IGroup group);

    }
}
