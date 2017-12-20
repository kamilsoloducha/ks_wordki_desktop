namespace Wordki.Models
{
    public interface ISettings
    {

        ISettings Load();
        void Save();

    }
}
