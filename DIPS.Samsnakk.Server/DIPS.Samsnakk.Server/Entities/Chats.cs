using DIPS.Samsnakk.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Entities
{
    public class Chats : IChats
    {
        public List<Chat> ChatList { get; set; } = new List<Chat>();
        public List<GroupChat> GroupChatList { get; set; } = new List<GroupChat>();

        public void AddChat(Chat chat)
        {
            ChatList.Add(chat);
        }

        public void AddGroupChat(GroupChat chat)
        {
            GroupChatList.Add(chat);
        }

        public Chat GetChat(string user1, string user2)
        {
            var chat = ChatList.FirstOrDefault(x => x.User1 == user1 && x.User2 == user2 || x.User1 == user2 && x.User2 == user1);

            if (chat == null)
            {
                Chat newChat = new Chat()
                {
                    User1 = user1,
                    User2 = user2
                };

                AddChat(newChat);

                chat = newChat;
            }

            return chat;
        }

        public GroupChat GetGroupChat(string groupName)
        {
            var chat = GroupChatList.FirstOrDefault(x => x.GroupName == groupName);

            if (chat == null)
            {
                GroupChat newChat = new GroupChat()
                {
                    GroupName = groupName
                };

                AddGroupChat(newChat);

                chat = newChat;
            }

            return chat;
        }
    }
}
