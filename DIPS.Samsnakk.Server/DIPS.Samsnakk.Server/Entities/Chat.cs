using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class Chat
    {
        public string User1 { get; set; }
        public string User2 { get; set; }

        public List<string> Messages { get; set; } = new List<string>();
    }
}
