using Repository.Models;
using System;

namespace Wordki.InteractionProvider
{
    public interface ICorrectWordProvider
    {
        IWord Word { get; set; }
        Action OnCorrect { get; set; }
        Action OnRemove { get; set; }
        Action OnClose { get; set; }

    }
}
