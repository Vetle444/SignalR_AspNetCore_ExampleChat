using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool Online { get; set; }
        public List<string> Notifications { get; set; } = new List<string>();

        public void AddNotification(string senderUsername)
        {
            if (!Notifications.Any(x => x == senderUsername))
                Notifications.Add(senderUsername);
        }
    }
}
