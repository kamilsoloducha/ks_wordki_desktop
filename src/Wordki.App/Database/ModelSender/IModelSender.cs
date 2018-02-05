using System.Collections.Generic;

namespace Wordki.Database
{
    public interface IModelSender<T>
    {
        IEnumerable<T> GetModelToSend();

    }
}
