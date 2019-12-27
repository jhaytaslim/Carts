using Carts.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carts.Data.Repository.Interface
{
    public interface IUserRepository : IRepository<UsersTb>
    {
        Task<UsersTb> GetUserByUsername(string username);
        Task<IEnumerable<UsersTb>> GetUsersByRole(string rolename);
        Task<UsersTb> ChangeActiveState(string id);
        new Task<UsersTb> Get(string id);
        Task<bool> SignIn(string username, string password);
        Task<bool> ChangePassword(string username, string password);
    }
}

