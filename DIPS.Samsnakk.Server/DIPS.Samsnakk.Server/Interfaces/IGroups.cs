using DIPS.Samsnakk.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Interfaces
{
    public interface IGroups
    {
        public List<Group> GetGroups();

        public void AddGroup(Group group);

        public List<Group> GetJoinedGroups(string username);

        public Group GetGroup(string groupName);


    }
}
