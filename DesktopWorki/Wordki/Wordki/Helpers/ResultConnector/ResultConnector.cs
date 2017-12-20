using WordkiModel;
using System.Linq;
using System.Collections.Generic;

namespace Wordki.Helpers.ResultConnector
{
    public class ResultConnector : IResultConnector
    {
        public ResultConnector()
        {
        }

        public bool Connect(IGroup dest, IGroup src)
        {
            if (!CheckLanguagesType(dest, src))
            {
                return false;
            }
            IList<IResult> bigger;
            IList<IResult> smaller;
            if (dest.Results.Count > src.Results.Count)
            {
                bigger = dest.Results;
                smaller = src.Results.ToList();
            }
            else
            {
                bigger = src.Results;
                smaller = dest.Results.ToList();
            };

            List<IResult> result = new List<IResult>();
            bool canConnect;
            IResult srcResult = null;
            for (int i = 0; i < bigger.Count; i++)
            {
                canConnect = false;
                foreach (IResult item in smaller)
                {
                    if (CanConnect(bigger[i], item))
                    {
                        canConnect = true;
                        srcResult = item;
                        break;
                    }
                }
                if (!canConnect)
                {
                    continue;
                }
                Connect(bigger[i], srcResult);
                smaller.Remove(srcResult);
                result.Add(bigger[i]);
            }
            dest.Results.Clear();
            foreach (IResult item in result)
            {
                dest.AddResult(item);
            }
            return true;
        }

        public void Connect(IResult dest, IResult src)
        {
            if (dest == null || src == null)
            {
                return;
            }
            dest.Correct += src.Correct;
            dest.Wrong += src.Wrong;
            dest.Accepted += src.Accepted;
            dest.Invisibilities += src.Invisibilities;
            dest.TimeCount += src.TimeCount;
        }

        private bool CheckLanguagesType(IGroup dest, IGroup src)
        {
            return dest.Language1 == src.Language1
                && dest.Language2 == src.Language2;
        }

        private bool CanConnect(IResult dest, IResult src)
        {
            return dest.LessonType == src.LessonType
                && dest.TranslationDirection == src.TranslationDirection;
        }
    }
}
