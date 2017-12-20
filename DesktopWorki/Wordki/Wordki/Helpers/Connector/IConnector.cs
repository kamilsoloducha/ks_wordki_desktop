namespace Wordki.Helpers.Connector
{
    public interface IConnector<T> where T : IResponse
    {

        IParser<T> Parser { get; set; }

        T SendRequest(IRequest request);

    }
}
