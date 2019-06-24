using Orleans;
using ServerEngine.Characters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerEngine.GrainInterfaces
{
    public interface IAccountGrain : IGrainWithStringKey
    {
        Task<string> GetUserName();

        Task<bool> IsClaimed();

        Task<bool> CanCreateNewPlayer();

        Task SetPassword(string password);

        Task<bool> ValidatePassword(string password);

        Task AddPermission(string permissionName);

        Task<bool> HasPermission(string permissionName);

        Task<List<string>> GetPermissions();

        Task AddCharacter(Guid characterTrackingId);

        Task<Character> GetCharacter(string characterName);
    }
}
