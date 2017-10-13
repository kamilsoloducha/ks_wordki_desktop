using Repository.Models;
using System.Collections.Generic;

namespace Wordki.Models.Lesson {
  public class BestLesson : RandomLesson{
    public BestLesson(IEnumerable<IWord> pWordsList) : base(pWordsList) {
    }
  }
}
