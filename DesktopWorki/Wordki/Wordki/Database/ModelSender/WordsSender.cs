using System.Collections.Generic;
using WordkiModel;
using Wordki.Database.Repositories;

namespace Wordki.Database
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
            foreach (IWord word in WordRepo.GetAll())
            {
                if (word.State != 0)
                {
                    yield return word;
                }
            }
        
        }
    }
}
