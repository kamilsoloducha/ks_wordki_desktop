using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.FileChooser
{
    public class FileChooser : IFileChooser
    {
        public string Choose()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Wybierz plik";
            bool? isOpen = fileDialog.ShowDialog();
            if (isOpen.HasValue && isOpen.Value)
            {
                return fileDialog.FileName;
            }
            return string.Empty;
        }
    }
}
