using DIPS.Samsnakk.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Interfaces
{
    public interface IChats
    {
        public void AddChat(Chat chat);
        public void AddGroupChat(GroupChat chat);
        public Chat GetChat(string user1Id, string user2Id);
        public GroupChat GetGroupChat(string groupName);
    }
}
