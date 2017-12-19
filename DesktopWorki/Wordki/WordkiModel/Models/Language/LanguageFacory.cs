using System.Collections.Generic;
using System.Linq;

namespace WordkiModel
{
    public class LanguageFactory
    {

        private static readonly Dictionary<LanguageType, ILanguage> Elements;
        static LanguageFactory()
        {
            Elements = new Dictionary<LanguageType, ILanguage>();
            Elements.Add(LanguageType.Default, new LanguageDefault());
            Elements.Add(LanguageType.English, new LanguageEnglish());
            Elements.Add(LanguageType.Polish, new LanguagePolish());
            Elements.Add(LanguageType.Germany, new LanguageGermany());
            Elements.Add(LanguageType.French, new LanguageFrench());
            Elements.Add(LanguageType.Spanish, new LanguageSpanish());
            Elements.Add(LanguageType.Portuaglese, new LanguagePortuguese());
            Elements.Add(LanguageType.Russian, new LanguageRussian());
            Elements.Add(LanguageType.Italian, new LanguageItalian());
        }

        public static ILanguage GetLanguage(LanguageType type)
        {
            if (!Elements.ContainsKey(type))
            {
                return Elements[LanguageType.Default];
            }
            return Elements[type];
        }

        public static IList<LanguageType> GetLanguagesTypes()
        {
            return System.Enum.GetValues(typeof(LanguageType)).Cast<LanguageType>().ToList();
        }

        public static IEnumerable<ILanguage> GetLanguages()
        {
            return Elements.Values;
        }
    }

    public class LanguageDefault : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Default; } }
        public string Code { get { return ""; } }
        public string Description { get { return "Język nieznany"; } }
        public string Name { get { return "Default"; } }
        public string ShortName { get { return "unknown"; } }
    }

    public class LanguageEnglish : ILanguage
    {
        public LanguageType Type { get { return LanguageType.English; } }
        public string Code { get { return "eng"; } }
        public string Description { get { return "Język angielski"; } }
        public string Name { get { return "English"; } }
        public string ShortName { get { return "uk"; } }
    }

    public class LanguagePolish : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Polish; } }
        public string Code { get { return "pol"; } }
        public string Description { get { return "Język polski"; } }
        public string Name { get { return "Polski"; } }
        public string ShortName { get { return "pl"; } }
    }

    public class LanguageGermany : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Germany; } }
        public string Code { get { return "deu"; } }
        public string Description { get { return "Język niemiecki"; } }
        public string Name { get { return "Deutsch"; } }
        public string ShortName { get { return "de"; } }
    }

    public class LanguageFrench : ILanguage
    {
        public LanguageType Type { get { return LanguageType.French; } }
        public string Code { get { return "fra"; } }
        public string Description { get { return "Język francuski"; } }
        public string Name { get { return "Français"; } }
        public string ShortName { get { return "fr"; } }
    }

    public class LanguageSpanish : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Spanish; } }
        public string Code { get { return "es"; } }
        public string Description { get { return "Język hiszpański"; } }
        public string Name { get { return "Español"; } }
        public string ShortName { get { return "es"; } }
    }

    public class LanguagePortuguese : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Portuaglese; } }
        public string Code { get { return "por"; } }
        public string Description { get { return "Język portugalski"; } }
        public string Name { get { return "Português"; } }
        public string ShortName { get { return "pt"; } }
    }

    public class LanguageRussian : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Russian; } }
        public string Code { get { return "rus"; } }
        public string Description { get { return "Język rosyjski"; } }
        public string Name { get { return "усский"; } }
        public string ShortName { get { return "ru"; } }
    }

    public class LanguageItalian : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Italian; } }
        public string Code { get { return "ita"; } }
        public string Description { get { return "Język włoski"; } }
        public string Name { get { return "Italiano"; } }
        public string ShortName { get { return "it"; } }
    }

}