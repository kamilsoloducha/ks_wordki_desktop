using System;
using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views
{
    /// <summary>
    /// Interaction logic for TeachPage.xaml
    /// </summary>
    public partial class TeachPage : ISwitchElement
    {

        private readonly IViewModel viewModel;

        public TeachPage(TeachPageType type)
        {
            InitializeComponent();
            viewModel = GetViewModel(type);
            DataContext = viewModel;
        }

        public IViewModel ViewModel { get { return viewModel; } }

        private IViewModel GetViewModel(TeachPageType type)
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
