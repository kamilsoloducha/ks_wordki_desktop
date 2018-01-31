using System;
using System.Windows;
using Wordki.Models.AppSettings;

namespace Wordki
{
    public partial class App : Application
    {

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhaldedException;

            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("nlog.config", true);
            string tep = AppSettingsSingleton.Instance.ApiHost;
        }

        private void OnUnhaldedException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Console.WriteLine($"=== UnhaldedException ===");
            Console.WriteLine($"Sender type: '{sender.GetType()}'");
            Console.WriteLine($"Exception type: '{ex.GetType()}'");
            Console.WriteLine($"Exception message: '{ex.Message}'");
            Console.WriteLine($"Exception stackTrace: '{ex.StackTrace}'");
        }
    }
}
