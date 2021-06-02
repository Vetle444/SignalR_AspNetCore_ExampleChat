using DIPS.Samsnakk.Client.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Client.Entities
{
    public class SignalRService : ISignalRService
    {
        private HubConnection connection;


        public SignalRService()
        {
            connection = new HubConnectionBuilder().WithUrl("https://localhost:5555/chatHub")
            .Build();

        }

        public async Task Connect()
        {
            await connection.StartAsync();
        }

        public HubConnection GetConnection()
        {
            return connection;
        }

        public async Task GetUsers()
        {
            await connection.SendAsync("GetUsers");
        }

        public async Task GetGroups()
        {
            await connection.SendAsync("GetGroups");
        }

        public async Task<string> Login(string username)
        {
            return await connection.InvokeAsync<string>("Login", username);
        }

        public async Task SendMessage(string receiver, string message)
        {
            await connection.SendAsync("SendMessage", receiver, message);
        }

        public async Task SendMessage(string groupName, string message, string sender)
        {
            await connection.SendAsync("SendGroupMessage", groupName, message, sender);
        }

        public async Task<List<string>> GetMessages(string username)
        {
            return await connection.InvokeAsync<List<string>>("GetMessages", username);
        }

        public async Task<List<string>> GetGroupMessages(string groupName)
        {
            return await connection.InvokeAsync<List<string>>("GetGroupMessages", groupName);
        }

        public async Task JoinGroup(string groupName)
        {
            await connection.SendAsync("JoinGroup", groupName);
        }

        public async Task AddGroup(string groupName)
        {
            await connection.SendAsync("AddGroup", groupName);
        }

        public async Task UpdateNotifications(List<string> notifications)
        {
            await connection.SendAsync("UpdateNotifications", notifications);
        }

        public async Task<List<string>> GetNotifications()
        {
            var notifications = await connection.InvokeAsync<List<string>>("GetNotifications");

            return notifications;
        }

    }
}
