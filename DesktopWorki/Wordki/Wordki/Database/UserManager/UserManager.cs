using WordkiModel;
using System.Threading.Tasks;

namespace Wordki.Database
{
    public class UserManager : IUserManager
    {
        private IDatabase database;
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
            database = DatabaseSingleton.Instance;
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
            database.UpdateUser(_user);
            
        }

        public Task UpdateAsync()
        {
            return database.UpdateUserAsync(_user);
        }
    }
}
