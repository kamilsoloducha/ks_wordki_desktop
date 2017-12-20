using System;
using Wordki.ViewModels;

namespace Wordki.Views
{
    /// <summary>
    /// Interaction logic for TeachPage.xaml
    /// </summary>
    public partial class TeachPage : PageBase
    {
        public TeachPage(TeachPageType type) : base(GetViewModel(type))
        {
            InitializeComponent();
        }

        private static IViewModel GetViewModel(TeachPageType type)
        {
            switch (type)
            {
                case TeachPageType.Typing:
                    {
                        return new TeachTypingViewModel();
                    }
                case TeachPageType.Fiszki:
                    {
                        return new TeachFiszkiViewModel();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum TeachPageType
    {
        Typing,
        Fiszki
    }
}
