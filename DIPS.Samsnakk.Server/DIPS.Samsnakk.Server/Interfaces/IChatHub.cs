using DIPS.Samsnakk.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Samsnakk.Server.Interfaces
{
    public interface IChatHub
    {
        Task ReceiveMessage(string message);

        Task ReceiveNotifications(List<string> notifications);

        Task OnGetUsers(List<User> users);

        Task OnGetGroups(List<Group> groups);

        Task OnGetJoinedGroups(List<Group> groupsJoined);
    }
}
