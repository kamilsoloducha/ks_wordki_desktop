using System;
using System.Text;

namespace Wordki.Helpers.WordComparer
{
    [Serializable]
    public class PunctuationNotCheck : INotCheck
    {
        private static StringBuilder _builder = new StringBuilder();

        public string Convert(string text)
        {
            _builder.Clear();
            foreach (char c in text.ToCharArray())
            {
                if (c == ' ' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= 128)
                {
                    _builder.Append(c);
                }

            }
            return _builder.ToString();
        }
    }
}
