using WordkiModel;

namespace Wordki.Commands
{
    public interface IWordSelectable
    {
        IWord SelectedWord { get; set; }
    }
}
