namespace Wordki.Models.AppSettings
{
    public class AppSettings : IAppSettings
    {
        public bool Debug { get; set; }
        public string ApiHost { get; set; }
        public bool DatabaseLog { get; set; }
        public bool DatabaseSeed { get; set; }
    }
}
