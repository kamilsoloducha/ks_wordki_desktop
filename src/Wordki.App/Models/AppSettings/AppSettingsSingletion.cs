using System;
using System.Configuration;

namespace Wordki.Models.AppSettings
{
    public static class AppSettingsSingletion
    {
        private static object lockObj = new object();

        private static IAppSettings instance;
        public static IAppSettings Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = "debug.config",
                        };
                        Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                        AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");
                        instance = new AppSettings();

                        bool temp;
                        if (section.Settings["debug"] == null || !Boolean.TryParse(section.Settings["debug"].Value, out temp))
                        {
                            temp = false;
                        }
                        instance.Debug = temp;

                        if (section.Settings["databaseLog"] == null || !Boolean.TryParse(section.Settings["databaseLog"].Value, out temp))
                        {
                            temp = false;
                        }
                        instance.DatabaseLog = temp;

                        if (section.Settings["databaseSeed"] == null || !Boolean.TryParse(section.Settings["databaseSeed"].Value, out temp))
                        {
                            temp = false;
                        }
                        instance.DatabaseSeed = temp;

                        instance.ApiHost = section.Settings["apiHost"] != null ? section.Settings["apiHost"].Value : string.Empty;
                    }
                }
                return instance;
            }
        }

    }
}
