using WordkiModel;
using System;
using System.Linq;
using Wordki.Commands;

namespace Wordki.InteractionProvider
{
    public class LanguageForGroupProvider : LanguageProvider
    {
        public int LanguageToChoose { get; set; }
        public IGroup GroupToChange { get; set; }

        public LanguageForGroupProvider() : base()
        {
            Items = Enum.GetValues(typeof(LanguageType)).Cast<LanguageType>();
        }

        public override void Interact()
        {
            viewModel.PositiveAction = () => ActionsSingleton<SelectLanguageAction>.Instance.Action(GroupToChange, LanguageToChoose, SelectedType);
            base.Interact();
        }

    }
}
