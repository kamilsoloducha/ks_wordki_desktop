
using WordkiModel;

namespace Wordki.Models.StateManager
{
    public class StateManager<T> : IStateManager<T>
    {

        public int NewState(int state, string name)
        {
            int propertyIndex = PropertyIndexAttribute.GetPropertyIndex(typeof(T), name);
            int value = 1 << propertyIndex;
            return state | value;
        }

        public int GetState(int state, string name)
        {
            int propertyIndex = PropertyIndexAttribute.GetPropertyIndex(typeof(T), name);
            return state & (1 << propertyIndex);
        }
    }
}
