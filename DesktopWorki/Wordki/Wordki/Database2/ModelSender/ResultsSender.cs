using Repository.Models;
using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Database2
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
            foreach(IResult result in ResultRepo.GetResults())
            {
                if(result.State != 0)
                {
                    yield return result;
                }
            }
        }
    }
}
