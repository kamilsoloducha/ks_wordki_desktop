using System;

namespace Wordki.Helpers.WordComparer
{
    [Serializable]
    public class LetterCaseNotCheck : INotCheck
    {
        public string Convert(string text)
        {
            return text.ToLower();
        }
    }
}
