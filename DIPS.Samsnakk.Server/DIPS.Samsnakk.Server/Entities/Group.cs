using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class Group
    {
        public string GroupName { get; set; }

        public List<string> Usernames { get; set; } = new List<string>();

    }
}
