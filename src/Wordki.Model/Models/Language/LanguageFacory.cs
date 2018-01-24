using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace WordkiModel
{
    public class LanguageFactory
    {

        private static readonly Dictionary<LanguageType, ILanguage> Elements;
        static LanguageFactory()
        {
            Elements = new Dictionary<LanguageType, ILanguage>
            {
                { LanguageType.Default, new LanguageDefault() },
                { LanguageType.English, new LanguageEnglish() },
                { LanguageType.Polish, new LanguagePolish() },
                { LanguageType.Germany, new LanguageGermany() },
                { LanguageType.French, new LanguageFrench() },
                { LanguageType.Spanish, new LanguageSpanish() },
                { LanguageType.Portuaglese, new LanguagePortuguese() },
                { LanguageType.Russian, new LanguageRussian() },
                { LanguageType.Italian, new LanguageItalian() }
            };
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

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageDefault()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguageEnglish : ILanguage
    {
        public LanguageType Type { get { return LanguageType.English; } }
        public string Code { get { return "eng"; } }
        public string Description { get { return "Język angielski"; } }
        public string Name { get { return "English"; } }
        public string ShortName { get { return "uk"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageEnglish()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguagePolish : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Polish; } }
        public string Code { get { return "pol"; } }
        public string Description { get { return "Język polski"; } }
        public string Name { get { return "Polski"; } }
        public string ShortName { get { return "pl"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguagePolish()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguageGermany : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Germany; } }
        public string Code { get { return "deu"; } }
        public string Description { get { return "Język niemiecki"; } }
        public string Name { get { return "Deutsch"; } }
        public string ShortName { get { return "de"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageGermany()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguageFrench : ILanguage
    {
        public LanguageType Type { get { return LanguageType.French; } }
        public string Code { get { return "fra"; } }
        public string Description { get { return "Język francuski"; } }
        public string Name { get { return "Français"; } }
        public string ShortName { get { return "fr"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageFrench()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguageSpanish : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Spanish; } }
        public string Code { get { return "es"; } }
        public string Description { get { return "Język hiszpański"; } }
        public string Name { get { return "Español"; } }
        public string ShortName { get { return "es"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageSpanish()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguagePortuguese : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Portuaglese; } }
        public string Code { get { return "por"; } }
        public string Description { get { return "Język portugalski"; } }
        public string Name { get { return "Português"; } }
        public string ShortName { get { return "pt"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguagePortuguese()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguageRussian : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Russian; } }
        public string Code { get { return "rus"; } }
        public string Description { get { return "Język rosyjski"; } }
        public string Name { get { return "усский"; } }
        public string ShortName { get { return "ru"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageRussian()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

    public class LanguageItalian : ILanguage
    {
        public LanguageType Type { get { return LanguageType.Italian; } }
        public string Code { get { return "ita"; } }
        public string Description { get { return "Język włoski"; } }
        public string Name { get { return "Italiano"; } }
        public string ShortName { get { return "it"; } }

        private BitmapImage flag;
        public BitmapImage Flag { get { return flag; } }

        public LanguageItalian()
        {
            flag = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FlagsCircle", string.Format("{0}.png", ShortName))));
        }
    }

}