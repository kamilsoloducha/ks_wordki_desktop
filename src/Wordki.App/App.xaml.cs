using System;
using System.Windows;

namespace Wordki
{
    public partial class App : Application
    {
        public App()
        {

            AppDomain.CurrentDomain.UnhandledException += OnUnhaldedException;

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
