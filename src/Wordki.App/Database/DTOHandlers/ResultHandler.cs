using AutoMapper;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Models;
using WordkiModel;
using WordkiModel.Extensions;

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
                if (group.Results.Any(x => x.Id == result.Id))
                {
                    group.AddResult(result);
                    database.UpdateResult(result);
                }
                else
                {
                    group.AddResult(result);
                    database.AddResult(result);
                }
            }
        }
    }
}
