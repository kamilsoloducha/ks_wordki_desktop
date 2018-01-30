using AutoMapper;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Database
{
    public class ResultHandler : IResultHandler
    {
        private readonly IMapper mapper;
        private readonly IDatabase database;

        public ResultHandler(IMapper mapper, IDatabase database)
        {
            this.mapper = mapper;
            this.database = database;
        }

        public void Handle(IEnumerable<ResultDTO> resultsDto)
        {
            foreach (ResultDTO resultDto in resultsDto)
            {
                IResult result = mapper.Map<ResultDTO, Result>(resultDto);
                IGroup group = database.Groups.FirstOrDefault(x => x.Id == resultDto.GroupId);
                if (result.State < 0)
                {
                    database.DeleteResult(result);
                }
                if (group.Words.Any(x => x.Id == result.Id))
                {
                    database.UpdateResult(result);
                }
                else
                {
                    database.AddResult(result);
                }
            }
        }
    }
}
