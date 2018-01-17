namespace Wordki.Helpers.Connector
{
    public interface IConnector<T>
    {

        IParser<T> Parser { get; set; }

        T SendRequest(IRequest request);

    }
}
