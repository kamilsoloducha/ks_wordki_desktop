using System.Text;
using System.Windows;
using Wordki.Helpers;
using Wordki.Models;

namespace Wordki
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private ISettings _settings = SettingsSingletion.Get();
        public ISettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        public App()
        {
            //Settings.Save();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoggerSingleton.LogInfo("Start aplikacji", null);

        }
    }
}
