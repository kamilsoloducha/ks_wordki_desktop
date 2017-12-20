using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wordki.Models;

namespace Wordki.Helpers.WordConnector
{
    public class WordConnector : IWordConnector
    {
        public void Connect(IEnumerable<Word> words)
        {
            if (words.Count() < 2)
            {
                return;
            }
            Word lSameWordDataGridItem = words.First();
            StringBuilder lLanguage1 = new StringBuilder();
            StringBuilder lLanguage2 = new StringBuilder();
            StringBuilder lLanguage1Comment = new StringBuilder();
            StringBuilder lLanguage2Comment = new StringBuilder();
            foreach (Word item in words)
            {
                if (!lLanguage1.ToString().Contains(item.Language1))
                {
                    lLanguage1.Append(item.Language1);
                    lLanguage1.Append(", ");
                }
                if (!lLanguage2.ToString().Contains(item.Language2))
                {
                    lLanguage2.Append(item.Language2);
                    lLanguage2.Append(", ");
                }
                if (!lLanguage1Comment.ToString().Contains(item.Language1Comment))
                {
                    lLanguage1Comment.Append(item.Language1Comment);
                    lLanguage1Comment.Append(". ");
                }
                if (!lLanguage2Comment.ToString().Contains(item.Language2Comment))
                {
                    lLanguage2Comment.Append(item.Language2Comment);
                    lLanguage2Comment.Append(". ");
                }
                item.State = -1;
            }
            lLanguage1.Remove(lLanguage1.Length - 2, 2);
            lLanguage2.Remove(lLanguage2.Length - 2, 2);

            lSameWordDataGridItem.Language1 = lLanguage1.ToString();
            lSameWordDataGridItem.Language2 = lLanguage2.ToString();
            lSameWordDataGridItem.Language1Comment = lLanguage1Comment.ToString();
            lSameWordDataGridItem.Language2Comment = lLanguage2Comment.ToString();
            lSameWordDataGridItem.Visible = true;
            lSameWordDataGridItem.Drawer = 0;
            lSameWordDataGridItem.State = int.MaxValue;
        }
    }
}
