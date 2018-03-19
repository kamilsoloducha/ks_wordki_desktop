using System.ComponentModel;
using System.Runtime.CompilerServices;
using WordkiModel;

namespace Wordki.Models
{
    public class GroupItem : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties


        private IGroup group;

        public IGroup Group
        {
            get { return group; }
            set {
                if(group == value)
                {
                    return;
                }
                group = value;
                OnPropertyChanged();
            }
        }

        private int daysToRepeat;
        public int DaysToRepeat
        {
            get { return daysToRepeat; }
            set
            {
                if (daysToRepeat == value)
                {
                    return;
                }
                daysToRepeat = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public GroupItem(IGroup group)
        {
            Group = group;
        }

    }
}
