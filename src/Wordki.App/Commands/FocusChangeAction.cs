using System;

namespace Wordki.Commands
{
    public class SetFocusAction
    {

        private readonly IFocusSelectable focusSelectable;
        private readonly int elementIndex;

        private Action action;
        public Action Action { get { return action; } }

        public SetFocusAction(IFocusSelectable focusSelectable, int elementIndex)
        {
            action = Execute;
            this.focusSelectable = focusSelectable;
            this.elementIndex = elementIndex;
        }

        private void Execute()
        {
            for (int i = 0; i < focusSelectable.Focusable.Count; i++)
            {
                focusSelectable.Focusable[i] = false;
            }
            focusSelectable.Focusable[elementIndex] = true;
        }

    }
}
