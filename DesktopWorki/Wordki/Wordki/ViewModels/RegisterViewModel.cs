using Repository.Model.DTOConverters;
using System;
using System.Windows.Input;
using Util.Threads;
using Wordki.Database;
using Wordki.Helpers.Connector;
using Wordki.Helpers.Connector.Requests;
using Wordki.Helpers.Connector.Work;
using Wordki.Models;
using WordkiModel;
using WordkiModel.DTO;

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

        public override void InitViewModel(object parameter = null)
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
                Console.WriteLine("Password and repetition is not same.");
                return;
            }
            IUser user = new User
            {
                Name = UserName,
                Password = Password
            };
            ApiWork<UserDTO> registerRequest = new ApiWork<UserDTO>
            {
                Request = new PostUsersRequest(user),
                OnCompletedFunc = RegisterComplete,
                OnFailedFunc = RegisterFailed
            };
            BackgroundWorkQueue queue = new BackgroundWorkQueue();
            queue.AddWork(registerRequest);
            queue.Execute();
        }

        private void RegisterFailed()
        {
        }

        private void RegisterComplete(UserDTO userDTO)
        {
            IUser user = UserConverter.GetModelFromDTO(userDTO);
            StartWithUser(user);
        }

        protected override void ChangeState(object obj)
        {
            Switcher.Reset();
        }

        #endregion

        //private void OnRegister(ApiResponse pResponse)
        //{
        //    //HandleResponse(pResponse);
        //}

        public override void Loaded()
        {
            
        }

        public override void Unloaded()
        {
            
        }
    }
}
