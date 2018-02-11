using AutoMapper;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Models;
using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Database
{
    public class WordHandler : IWordHandler
    {
        private readonly IMapper mapper;
        private readonly IDatabase database;

        public WordHandler(IMapper mapper, IDatabase database)
        {
            this.mapper = mapper;
            this.database = database;
        }

        public void Handle(IEnumerable<WordDTO> wordsDto)
        {
            foreach (WordDTO wordDto in wordsDto)
            {
                IWord word = mapper.Map<WordDTO, Word>(wordDto);
                IGroup group = database.Groups.FirstOrDefault(x => x.Id == wordDto.GroupId);
                if (wordDto.State < 0)
                {
                    database.DeleteWord(word);
                }
                if (group.Words.Any(x => x.Id == word.Id))
                {
                    group.AddWord(word);
                    database.UpdateWord(word);
                }
                else
                {
                    group.AddWord(word);
                    database.AddWord(word);
                }
            }
        }
    }
}
