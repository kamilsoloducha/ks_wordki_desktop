using AutoMapper;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Database.Repositories;
using Wordki.Models;
using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Database
{
    public class WordHandler : IWordHandler
    {
        private readonly IMapper mapper;
        private readonly IDatabase database;
        private readonly IWordRepository wordRepository;

        public WordHandler(IMapper mapper, IDatabase database)
        {
            this.mapper = mapper;
            this.database = database;
            this.wordRepository = new WordRepository();
        }

        public void Handle(IEnumerable<WordDTO> wordsDto)
        {
            database.LoadDatabase();
            IList<IWord> toUpdate = new List<IWord>();
            IList<IWord> toAdd = new List<IWord>();
            foreach (WordDTO wordDto in wordsDto)
            {
                IWord word = mapper.Map<WordDTO, Word>(wordDto);
                IGroup group = database.Groups.FirstOrDefault(x => x.Id == wordDto.GroupId);
                if (wordDto.State < 0 && group.Words.Any(x => x.Id == word.Id))
                {
                    toUpdate.Add(word);
                }
                if (group.Words.Any(x => x.Id == word.Id))
                {
                    group.AddWord(word);
                    toUpdate.Add(word);
                }
                else
                {
                    group.AddWord(word);
                    toAdd.Add(word);
                }
            }
            wordRepository.Update(toUpdate);
            wordRepository.Save(toAdd);
        }
    }
}
