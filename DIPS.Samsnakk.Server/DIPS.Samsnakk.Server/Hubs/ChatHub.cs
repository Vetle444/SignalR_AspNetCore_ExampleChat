using DIPS.Samsnakk.Server.Entities;
using DIPS.Samsnakk.Server.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        private IUsers Users { get; set; }
        private IChats Chats { get; set; }
        private IGroups _groups { get; set; }

        public ChatHub(IUsers users, IChats chats, IGroups groups)
        {
            Users = users;
            Chats = chats;
            _groups = groups;
        }

        // Sends a message to a user

        public async Task SendMessage(string receiverUsername, string message)
        {
            var userSentMessage = Users.GetCurrentUser(Context.ConnectionId);
            var userReceivedMessage = Users.GetUserByUsername(receiverUsername);

            message = $"{userSentMessage.Username}: {message}";

            // Add notification from sender to receiver
            userReceivedMessage.AddNotification(userSentMessage.Username);

            // If user is online, send the message directly to him
            if (userReceivedMessage.Online)
                await Clients.Client(userReceivedMessage.Id).ReceiveMessage(message);

            // Send notifications to receiver
            await Clients.Client(userReceivedMessage.Id).ReceiveNotifications(userReceivedMessage.Notifications);

            // Send the message to the one who called the function
            await Clients.Caller.ReceiveMessage(message);

            // Gets the chat between the users and adds the new message to the messages list
            var chat = Chats.GetChat(userSentMessage.Username, userReceivedMessage.Username);
            chat.Messages.Add(message);
        }

        // Sends a group message

        public async Task SendGroupMessage(string groupName, string message, string sender)
        {
            // Get the group by its group name
            var group = _groups.GetGroup(groupName);

            message = $"{sender}: {message}";

            // Send message to all members of this SignalrR-group
            await Clients.Group(groupName).ReceiveMessage(message);

            // Get the corresponding group chat for this group
            var groupChat = Chats.GetGroupChat(groupName);

            // For every user of the group, add a notification and notify user of incoming notifications
            foreach(string username in group.Usernames)
            {
                var user = Users.GetUserByUsername(username);

                // Don't notify the caller
                if (user.Id == Context.ConnectionId)
                    continue;

                user.AddNotification(groupName);

                await Clients.Client(user.Id).ReceiveNotifications(user.Notifications);
            }

            groupChat.Messages.Add(message);
        }

        // Get all users in the in-memory database

        public async Task GetUsers()
        {
            await Clients.Client(Context.ConnectionId).OnGetUsers(Users.GetUsers());
        }

        // Gets all groups in the in-memory database

        public async Task GetGroups()
        {
            var user = Users.GetCurrentUser(Context.ConnectionId);

            // Gets all the groups that user has joined
            var groupsJoined = _groups.GetJoinedGroups(user.Username);

            // Gives the user who called the function, all groups aswell as its joined groups from in-memory database
            await Clients.Caller.OnGetGroups(_groups.GetGroups());
            await Clients.Caller.OnGetJoinedGroups(groupsJoined);
        }

        public async Task<string> Login(string username)
        {
            var user = Users.GetUsers().FirstOrDefault(x => x.Username == username);

            // If user does not exist in the in-memory database, create the user
            if(user == null)
            {
                user = new User()
                {
                    Username = username
                };

                Users.AddUser(user);
            }

            user.Id = Context.ConnectionId;
            user.Online = true;

            // Gives all users who is in the 'UsersLoggedIn' group all groups and users
            await Clients.Group("UsersLoggedIn").OnGetGroups(_groups.GetGroups());
            await Clients.Group("UsersLoggedIn").OnGetUsers(Users.GetUsers());

            // Caller joins the group 'UsersLoggedIn'
            await Groups.AddToGroupAsync(user.Id, "UsersLoggedIn");

            // Joins all the SignalR groups that he has previously joined
            var joinedGroups = _groups.GetJoinedGroups(user.Username);
            foreach(Group group in joinedGroups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupName);
            }

            return user.Username;
        }

        // Gets all messages between caller and the user which is specified in parameter 
        public List<string> GetMessages(string username)
        {
            var user = Users.GetCurrentUser(Context.ConnectionId);

            var chat = Chats.GetChat(user.Username, username);

            return chat.Messages;
        }

        // Gets all the group messages, for the group specified in parameter
        public List<string> GetGroupMessages(string groupName)
        {
            var chat = Chats.GetGroupChat(groupName);

            return chat.Messages;
        }

        // Gets all the notifications related to the caller
        public List<string> GetNotifications()
        {
            var user = Users.GetCurrentUser(Context.ConnectionId);

            return user.Notifications;
        }

        // Called joins the group specified in parameter
        public async Task JoinGroup(string groupName)
        {
            // Gets the current caller's user 
            var user = Users.GetCurrentUser(Context.ConnectionId);

            // Joins the SignalR-group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Gets specified group in the in-memory database
            var group = _groups.GetGroups().Find(x => x.GroupName == groupName);

            // Adds the callers user to the group
            group.Usernames.Add(user.Username);

            // Retrieves the caller's joined groups from in-memory database
            var groupsJoined = _groups.GetJoinedGroups(user.Username);

            // Caller gets its joined groups from server to client
            await Clients.Caller.OnGetJoinedGroups(groupsJoined);
        }

        // Creates a new group in the in-memory database
        public async Task AddGroup(string groupName)
        {
            var group = new Group()
            {
                GroupName = groupName
            };

            _groups.AddGroup(group);

            // Notifies all logged in clients that a new group has been created, and sends all groups from in-memory database
            await Clients.Group("UsersLoggedIn").OnGetGroups(_groups.GetGroups());
        }

        // Sends all caller's notifications to client
        public void UpdateNotifications(List<string> notifications)
        {
            var user = Users.GetCurrentUser(Context.ConnectionId);

            user.Notifications = notifications;
        }

        // If client disconnects
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = Users.GetCurrentUser(Context.ConnectionId);
            user.Online = false;

            var joinedGroups = _groups.GetJoinedGroups(user.Username);

            // Cleanup, remove the disconnected user from all SignalR groups
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "UsersLoggedIn");

            foreach(Group group in joinedGroups)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group.GroupName);
            }

            // Notifies all logged in clients that a user has gone offline, and sends all users from in-memory database
            await Clients.Group("UsersLoggedIn").OnGetUsers(Users.GetUsers());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
