using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Client.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool Online { get; set; }
    }
}
