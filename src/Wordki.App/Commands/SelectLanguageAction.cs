using WordkiModel;
using System;

namespace Wordki.Commands
{
    public class SelectLanguageAction
    {

        private Action<IGroup, int, LanguageType> action;
        public Action<IGroup, int, LanguageType> Action { get { return action; } }

        public SelectLanguageAction()
        {
            action = Execute;
        }

        private void Execute(IGroup group, int languageToChoose, LanguageType selectedType)
        {
            if (group == null)
            {
                throw new ArgumentNullException("Parameter group is null");
            }
            if(languageToChoose != 1 && languageToChoose != 2)
            {
                throw new ArgumentOutOfRangeException("Parameter languageToChoose are not equal 1 or 2");
            }
            if (languageToChoose == 1)
            {
                group.Language1 = selectedType;
            }
            else
            {
                group.Language2 = selectedType;
            }
        }
    }
}
