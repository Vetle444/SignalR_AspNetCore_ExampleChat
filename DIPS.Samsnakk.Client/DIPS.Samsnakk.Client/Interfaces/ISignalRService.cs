using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Client.Interfaces
{
    public interface ISignalRService
    {
        public Task Connect();

        public HubConnection GetConnection();

        public Task GetUsers();
        public Task GetGroups();
        public Task<string> Login(string username);
        public Task SendMessage(string receiver, string message);
        public Task SendMessage(string groupName, string message, string sender);

        public Task<List<string>> GetMessages(string username);
        public Task<List<string>> GetGroupMessages(string groupName);

        public Task JoinGroup(string groupName);

        public Task AddGroup(string groupName);

        public Task UpdateNotifications(List<string> notifications);

        public Task<List<string>> GetNotifications();

    }
}
