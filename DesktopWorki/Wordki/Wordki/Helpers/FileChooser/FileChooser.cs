using Microsoft.Win32;

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
