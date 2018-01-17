using System.Collections.Generic;
using Wordki.Models;
using WordkiModel;
using WordkiModel.DTO;

namespace Repository.Model.DTOConverters
{
    public static class ResultConverter
    {

        public static ResultDTO GetDTOFromModel(IResult result)
        {
            return new ResultDTO()
            {
                Id = result.Id,
                GroupId = result.Group.Id,
                Correct = result.Correct,
                Accepted = result.Accepted,
                Wrong = result.Wrong,
                Invisible= result.Invisible,
                TimeCount = result.TimeCount,
                TranslationDirection = result.TranslationDirection,
                LessonType= result.LessonType,
                DateTime = result.DateTime,
                State = result.State,
            };
        }

        public static IResult GetModelFromDTO(ResultDTO result)
        {
            return new Result()
            {
                Id = result.Id,
                Group = new Group() { Id = result.GroupId },
                Correct = result.Correct,
                Accepted = result.Accepted,
                Wrong = result.Wrong,
                Invisible = result.Invisible,
                TimeCount = result.TimeCount,
                TranslationDirection = result.TranslationDirection,
                LessonType = result.LessonType,
                DateTime = result.DateTime,
                State = result.State,
            };
        }

        public static IEnumerable<IResult> GetResultsFromDTOs(IEnumerable<ResultDTO> results)
        {
            foreach(ResultDTO result in results)
            {
                yield return GetModelFromDTO(result);
            }
        }

        public static IEnumerable<ResultDTO> GetDTOsFromResults(IEnumerable<IResult> results)
        {
            foreach (IResult result in results)
            {
                yield return GetDTOFromModel(result);
            }
        }
    }
}
