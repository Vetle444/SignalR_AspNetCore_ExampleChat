using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class GroupChat
    {
        public string GroupName { get; set; }

        public List<string> Messages { get; set; } = new List<string>();
    }
}
