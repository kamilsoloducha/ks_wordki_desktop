using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordki.Models.Lesson.WordComparer {
  [Serializable]
  public class WordComparer : IWordComparer{

    public bool FontSizeSensitive { get; set; }

    public bool Compare(string word1, string word2) {
      List<string> lOriginalList = word1.Split(',').ToList();
      List<string> lTranslationList = word2.Split(',').ToList();
      for (int i = 0; i < lOriginalList.Count; i++) {
        lOriginalList[i] = ConvertUtfToAscii(lOriginalList[i].Trim());
      }
      for (int i = 0; i < lTranslationList.Count; i++) {
        lTranslationList[i] = ConvertUtfToAscii(lTranslationList[i].Trim());
      }
      foreach (string lOriginalWord in lOriginalList) {
        foreach (string lTranslationWord in lTranslationList) {
          if (InsideCompare(lOriginalWord, lTranslationWord)) return true;
        }
      }
      return false;
    }

    private bool InsideCompare(string word1, string word2) {
      if (!FontSizeSensitive) {
        return word1.ToLower().Equals(word2.ToLower());
      }
      return word1.Equals(word2);
    }

    private string ConvertUtfToAscii(string pString) {
      List<char> lInputList = pString.ToCharArray().ToList();
      List<char> lOutputList = new List<char>();
      foreach (char lSign in lInputList) {
        if (lSign > 255) {
          continue;
        }
        lOutputList.Add(lSign);
      }
      return new string(lOutputList.ToArray());
    }
  }
}
