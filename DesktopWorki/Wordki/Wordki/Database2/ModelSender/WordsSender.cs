using System.Collections.Generic;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Database2
{
    public class WordsSender : IModelSender<IWord>
    {

        public IWordRepository WordRepo { get; set; }

        public WordsSender()
        {
            WordRepo = new WordRepository();
        }

        public IEnumerable<IWord> GetModelToSend()
        {
            foreach (IWord word in WordRepo.GetWords())
            {
                if (word.State != 0)
                {
                    yield return word;
                }
            }
        
        }
    }
}
