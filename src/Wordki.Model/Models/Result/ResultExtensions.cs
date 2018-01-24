using WordkiModel.Enums;

namespace WordkiModel
{
    public static class ResultExtensions
    {

        public static void ChangeDirection(this IResult result)
        {
            if (result.TranslationDirection == TranslationDirection.FromFirst)
            {
                result.TranslationDirection = TranslationDirection.FromSecond;
            }
            else
            {
                result.TranslationDirection = TranslationDirection.FromFirst;
            }
        }

    }
}
