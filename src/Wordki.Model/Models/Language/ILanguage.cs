#if NETSTANDARD2_0
#else
using System.Windows.Media.Imaging;
#endif

namespace WordkiModel
{
    public interface ILanguage
    {

        LanguageType Type { get; }
        string Code { get; }
        string Description { get; }
        string Name { get; }
        string ShortName { get; }
#if NETSTANDARD2_0
#else
        BitmapImage Flag { get; }
#endif


    }
}