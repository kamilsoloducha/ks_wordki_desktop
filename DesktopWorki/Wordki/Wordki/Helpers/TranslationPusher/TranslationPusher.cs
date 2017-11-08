using System.Collections.Generic;
using Repository.Models;
using Repository.Models.Enums;
using System.Text;

namespace Wordki.Helpers.TranslationPusher
{
    public class TranslationPusher : ITranslationPusher
    {

        public IEnumerable<string> TranslationsList { get; set; }
        public TranslationDirection TranslationDirection { get; set; }

        public void SetTranslation(IWord word)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string item in TranslationsList)
            {
                builder.Append(item).Append(", ");
            }
            builder.Remove(builder.Length - 2, 2);
            if (TranslationDirection == TranslationDirection.FromFirst)
            {
                word.Language2 = builder.ToString();
            }
            else
            {
                word.Language1 = builder.ToString();
            }
        }
    }
}
