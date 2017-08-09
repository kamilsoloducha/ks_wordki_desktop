using System.Collections.Generic;

namespace Wordki.Models.Lesson {
  public class BestLesson : RandomLesson{
    public BestLesson(IEnumerable<Word> pWordsList) : base(pWordsList) {
    }
  }
}
