using WordkiModel;

namespace Wordki.Helpers.ResultConnector
{
    public interface IResultConnector
    {

        bool Connect(IGroup dest, IGroup src);

        void Connect(IResult dest, IResult src);

    }
}
