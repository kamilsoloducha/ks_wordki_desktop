namespace Wordki.Helpers.Connector
{
    public interface IParser<T> where T : IResponse
    {

        T Parse(string message);

    }
}
