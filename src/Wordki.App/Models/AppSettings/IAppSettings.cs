namespace Wordki.Models.AppSettings
{
    public interface IAppSettings
    {
        string ApiHost { get; set; }
        bool Debug { get; set; }
        bool DatabaseLog { get; set; }
        bool DatabaseSeed { get; set; }
    }
}