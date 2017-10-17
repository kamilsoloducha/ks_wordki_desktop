using Repository.Models;
using Wordki.Models;

namespace Wordki.Database2
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
            _userRepo.UpdateAsync(_user);
        }
    }
}
