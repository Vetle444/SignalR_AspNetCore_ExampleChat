using DIPS.Samsnakk.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Interfaces
{
    public interface IUsers
    {
        public void AddUser(User user);
        public List<User> GetUsers();

        public User GetCurrentUser(string id);

        public User GetUserByUsername(string username);

    }
}
