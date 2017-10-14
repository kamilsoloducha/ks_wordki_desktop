using System.Collections.Generic;

namespace Wordki.Database2
{
    public interface IModelSender<T>
    {
        IEnumerable<T> GetModelToSend();

    }
}
