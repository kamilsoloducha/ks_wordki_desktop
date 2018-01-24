namespace WordkiModel
{
    public static class GroupExtensions
    {
        public static void SwapLanguage(this IGroup group)
        {
            LanguageType temp = group.Language1;
            group.Language1 = group.Language2;
            group.Language2 = temp;
            foreach (var word in group.Words)
            {
                word.SwapLanguage();
            }
            foreach (var result in group.Results)
            {
                result.ChangeDirection();
            }
        }
    }
}
