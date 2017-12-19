namespace WordkiModel
{
    public static class WordExtenstions
    {

        public static void ChangeVisibility(this IWord word)
        {
            word.Visible = !word.Visible;
        }

        public static void ResetDrawer(this IWord word)
        {
            word.Drawer = 0;
        }

        public static void IncreadDrawer(this IWord word)
        {
            word.Drawer++;
        }

        public static void ChagnedSelected(this IWord word)
        {
            word.Selected = !word.Selected;
        }

        public static void SwapLanguage(this IWord word)
        {
            string temp = word.Language1;
            word.Language1 = word.Language2;
            word.Language2 = temp;

            temp = word.Language1Comment;
            word.Language1Comment = word.Language2Comment;
            word.Language2Comment = temp;
        }

    }
}
