using DIPS.Samsnakk.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class Users : IUsers
    {
        private List<User> UserList { get; set; } = new List<User>();

        public void AddUser(User user)
        {
            UserList.Add(user);
        }

        public List<User> GetUsers()
        {
            return UserList;
        }

        public User GetCurrentUser(string id)
        {
            var user = UserList.Find(x => x.Id == id);

            return user;
        }

        public User GetUserByUsername(string username)
        {
            var user = UserList.Find(x => x.Username == username);

            return user;
        }
    }
}
