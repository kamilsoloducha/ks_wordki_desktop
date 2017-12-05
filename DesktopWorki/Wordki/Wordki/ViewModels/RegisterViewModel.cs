using System;
using System.Windows.Input;
using Wordki.Models;
using Wordki.Models.Connector;

namespace Wordki.ViewModels
{
    public class RegisterViewModel : LoginMainViewModel
    {

        #region Properties

        private string _repeatPassword;
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructors

        public RegisterViewModel()
        {
            RegisterCommand = new Util.BuilderCommand(Register);
        }

        #endregion

        public override void InitViewModel()
        {
            base.InitViewModel();
            RepeatPassword = "";
        }

        #region Commands

        private void Register(object obj)
        {
            if (Password == null)
                return;
            if (RepeatPassword == null)
                return;
            if (!Password.Equals(RepeatPassword))
            {
                //Parent.ShowToast("Wprowadzone hasła nie są identyczne", ToastLevel.Alert);
                return;
            }
            User user = new User
            {
                Name = UserName,
                Password = Password
            };
            //CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
            //lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestRegister(user)) { OnCompleteCommand = OnRegister });
            //lQueue.Execute();
        }

        protected override void ChangeState(object obj)
        {
            Switcher.Reset();
        }

        #endregion

        private void OnRegister(ApiResponse pResponse)
        {
            HandleResponse(pResponse);
        }

        public override void Loaded()
        {
            
        }

        public override void Unloaded()
        {
            
        }
    }
}
