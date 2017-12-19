using WordkiModel;
using System.Collections.Generic;
using Wordki.Database.Repositories;
using Wordki.Models;

namespace Wordki.Database
{
    public class ResultsSender : IModelSender<IResult>
    {

        public IResultRepository ResultRepo { get; set; }

        public ResultsSender()
        {
            ResultRepo = new ResultRepository();
        }

        public IEnumerable<IResult> GetModelToSend()
        {
            foreach(IResult result in ResultRepo.GetAll())
            {
                if(result.State != 0)
                {
                    yield return result;
                }
            }
        }
    }
}
