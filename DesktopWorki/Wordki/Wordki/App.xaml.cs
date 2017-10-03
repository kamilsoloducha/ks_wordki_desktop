using System;
using System.Text;
using System.Windows;
using Wordki.Helpers;
using Wordki.LocalDatabase;

namespace Wordki
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SqliteConnection lConnection = new SqliteConnection();
            lConnection.CreateDatabase();
            lConnection.CreateTables();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Logger.LogInfo("Start aplikacji", null);

        }
    }
}
