
namespace Wordki.Models.Lesson.WordComparer {
  public interface IWordComparer {

    bool FontSizeSensitive { get; set; }

    bool Compare(string word1, string word2);

  }
}
