using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Util.Serializers;

namespace Wordki.Models
{
    [Serializable]
    public class Settings2 : ISettings, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChaged([CallerMemberName]string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion


        private int _fontSize;

        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize == value)
                {
                    return;
                }
                _fontSize = value;
                OnPropertyChaged();
            }
        }



        private static string _path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Wordki", "settings.dat");

        public Settings2()
        {
            FontSize = 30;
        }


        public ISettings Load()
        {
            ISerializer<ISettings> serializer = new BinarySerializer<ISettings>()
            {
                Settings = new BinarySerializerSettings()
                {
                    Path = _path,
                    RemoveAfterRead = false,
                }
            };
            return serializer.Read();
        }

        public void Save()
        {
            ISerializer<ISettings> serializer = new BinarySerializer<ISettings>()
            {
                Settings = new BinarySerializerSettings()
                {
                    Append = false,
                    Path = _path,
                }
            };
            serializer.Write(this);
        }
    }
}
