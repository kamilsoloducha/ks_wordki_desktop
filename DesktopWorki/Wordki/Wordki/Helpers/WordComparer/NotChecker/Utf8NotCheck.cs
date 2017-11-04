using System;
using System.Collections.Generic;
using System.Text;

namespace Wordki.Helpers.WordComparer
{
    [Serializable]
    public class Utf8NotCheck : INotCheck
    {

        private static Dictionary<char, char> _dictionary = new Dictionary<char, char>();
        private static StringBuilder _builder = new StringBuilder();

        static Utf8NotCheck()
        {
            _dictionary.Add('ą', 'a');
            _dictionary.Add('ś', 's');
            _dictionary.Add('ę', 'e');
            _dictionary.Add('ż', 'z');
            _dictionary.Add('ź', 'z');
            _dictionary.Add('ć', 'c');
            _dictionary.Add('ł', 'l');
            _dictionary.Add('ó', 'o');
            _dictionary.Add('ń', 'n');

            _dictionary.Add('Ą', 'A');
            _dictionary.Add('Ś', 'S');
            _dictionary.Add('Ę', 'E');
            _dictionary.Add('Ż', 'Z');
            _dictionary.Add('Ź', 'Z');
            _dictionary.Add('Ć', 'C');
            _dictionary.Add('Ł', 'L');
            _dictionary.Add('Ó', 'O');
            _dictionary.Add('Ń', 'N');
        }

        public string Convert(string text)
        {
            _builder.Clear().Append(text);
            foreach (char c in text.ToCharArray())
            {
                if (c > 128)
                {
                    _builder.Replace(c, _dictionary[c]);
                }
            }
            return _builder.ToString();
        }

    }
}
