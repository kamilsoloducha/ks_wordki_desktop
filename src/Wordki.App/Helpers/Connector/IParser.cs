namespace Wordki.Helpers.Connector
{
    public interface IParser<T>
    {
        T Parse(string message);
    }
}
