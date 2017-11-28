using Repository.Models;
using System;
using System.Collections.Generic;

namespace Wordki.InteractionProvider
{
    public interface ILessonResultProvider
    {
        IEnumerable<IResult> Results { get; set; }
        Action OnClose { get; set; }
    }
}