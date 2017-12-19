using System.IO;
using WordkiModel;

namespace Wordki.Models
{
    public class LanguageIconManager
    {
        public static string GetPathCircleFlag(ILanguage language)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Icons", "FlagsCircle", string.Format("{0}.png", language.ShortName));
        }

        public static string GetPathRectFlag(ILanguage language)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Icons", "Flags", string.Format("{0}.gif", language.ShortName));
        }
    }
}
