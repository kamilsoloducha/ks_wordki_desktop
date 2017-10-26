using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Helpers.WordComparer
{
    public class WordComparer : IWordComparer
    {
        public bool IsEqual(Word word1, Word word2)
        {
            int isSame = 0;
            if (word1.Id == word2.Id)
                return false;
            if (isSame == 0 && word1.Language1.Equals(word2.Language1))
                isSame++;
            if (isSame == 0 && word1.Language2.Equals(word2.Language2))
                isSame++;
            if (isSame == 0 && IsPartialSame(word1.Language1, word2.Language1))
                isSame++;
            if (isSame == 0 && IsPartialSame(word1.Language2, word2.Language2))
                isSame++;
            return isSame == 0;
        }

        private bool IsPartialSame(string lWord1, string lWord2)
        {
            try
            {
                string[] lWord1Array = lWord1.Split(' ');
                string[] lWord2Array = lWord2.Split(' ');
                if (lWord1Array.Count() == 1 || lWord2Array.Count() == 1)
                {
                    return false;
                }
                for (int i = 0; i < lWord1Array.Count(); i++)
                {
                    lWord1Array[i] = lWord1Array[i].Trim(',', '.', '-', '\\', '/');
                }
                for (int i = 0; i < lWord2Array.Count(); i++)
                {
                    lWord2Array[i] = lWord2Array[i].Trim(',', '.', '-', '\\', '/');
                }
                if (lWord1Array.Where(lItem1 => lItem1.Length >= 4).Any(lItem1 => lWord2Array.Where(lItem2 => lItem2.Length >= 4).Any(lItem1.Equals)))
                {
                    return true;
                }
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1}", "BuilderViewModel.IsPartialSame", lException.Message);
            }
            return false;
        }
    }
}
