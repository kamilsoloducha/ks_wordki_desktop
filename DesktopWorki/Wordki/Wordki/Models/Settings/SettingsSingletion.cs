namespace Wordki.Models
{
    public static class SettingsSingletion
    {

        private static ISettings _instance;

        private static object _lock = new object();

        public static ISettings Get()
        {
            lock (_lock)
            {
                if(_instance == null)
                {
                    _instance = new Settings2();
                    ISettings loaded = _instance.Load();
                    if (loaded != null)
                    {
                        _instance = loaded;
                    }
                }
                return _instance;
            }
        }

    }
}
