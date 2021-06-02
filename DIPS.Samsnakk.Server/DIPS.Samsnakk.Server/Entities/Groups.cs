using DIPS.Samsnakk.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class Groups : IGroups
    {
        public List<Group> GroupList { get; set; } = new List<Group>();

        public List<Group> GetGroups()
        {
            return GroupList;
        }

        public void AddGroup(Group group)
        {
            GroupList.Add(group);
        }

        public Group GetGroup(string groupName)
        {
            return GroupList.Find(x => x.GroupName == groupName);
        }

        public List<Group> GetJoinedGroups(string username)
        {
            var groupsJoined = GroupList.Where(x => x.Usernames.Any(x => x == username)).ToList();

            return groupsJoined;
        }

    }
}
