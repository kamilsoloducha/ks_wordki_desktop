using System.Collections.Generic;
using System.Text;

namespace Wordki.Models.Lesson.WordComparer
{
    public class Utf8NotCheck : INotCheck
    {

        private static Dictionary<char, char> _dictionary = new Dictionary<char, char>();

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

        public StringBuilder Convert(StringBuilder text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 128)
                {
                    text.Replace(text[i], _dictionary[text[i]]);
                }
            }
            return text;
        }

    }
}
