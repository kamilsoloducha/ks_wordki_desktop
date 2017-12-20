using WordkiModel;
using System.Threading.Tasks;
using Wordki.Database.Repositories;

namespace Wordki.Database
{
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepo;
        private IUser _user;
        public IUser User
        {
            get
            {
                return _user;
            }
        }

        public UserManager()
        {
            _userRepo = new UserRepository();
        }

        public void Set(IUser user)
        {
            _user = user;
        }

        public void UnSet()
        {
            _user = null;
        }

        public void Update()
        {
            _userRepo.Update(_user);
        }

        public Task UpdateAsync()
        {
            return _userRepo.UpdateAsync(_user);
        }
    }
}
