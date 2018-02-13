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
    public class ResultHandler : IResultHandler
    {
        private readonly IMapper mapper;
        private readonly IDatabase database;
        private readonly IResultRepository resultRepositry;

        public ResultHandler(IMapper mapper, IDatabase database)
        {
            this.mapper = mapper;
            this.database = database;
            this.resultRepositry = new ResultRepository();
        }

        public void Handle(IEnumerable<ResultDTO> resultsDto)
        {
            database.LoadDatabase();
            IList<IResult> toUpdate = new List<IResult>();
            IList<IResult> toAdd = new List<IResult>();
            foreach (ResultDTO resultDto in resultsDto)
            {
                IResult result = mapper.Map<ResultDTO, Result>(resultDto);
                IGroup group = database.Groups.FirstOrDefault(x => x.Id == resultDto.GroupId);
                if (result.State < 0)
                {
                    toUpdate.Add(result);
                }
                if (group.Results.Any(x => x.Id == result.Id))
                {
                    group.AddResult(result);
                    toUpdate.Add(result);
                }
                else
                {
                    group.AddResult(result);
                    toAdd.Add(result);
                }
            }
            resultRepositry.Update(toUpdate);
            resultRepositry.Save(toAdd);
        }
    }
}
