using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carts.Data.POCO;
using Carts.Data.Repository.Interface;

namespace Carts.Data.Repository
{
    public class UserRepository : Repository<UsersTb>, IUserRepository
    {
        public UserRepository(DbContext context, ILogger<dynamic> log) : base(context, log)
        {
        }

        private AirTableContext _appContext => (AirTableContext)_context;

        private ILogger<dynamic> logger => (ILogger<dynamic>)_log;

        public async Task<UsersTb> GetUserByUsername(string username) => await _appContext.UsersTb.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<bool> SignIn(string username, string password)
        {
            try
            {
                var user = await _appContext.UsersTb.Include(u => u.Role)
                            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

                if(user != null)
                {
                    user.LastLoginDate = DateTime.Now;
                    _appContext.UsersTb.Update(user);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ChangePassword(string username, string password)
        {
            try
            {
                var user = await _appContext.UsersTb.Where(u => u.Username == username).SingleOrDefaultAsync();

                if(user != null)
                {
                    user.Password = password;
                    _appContext.UsersTb.Update(user);
                    await _appContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<UsersTb>> GetUsersByRole(string rolename) => await _appContext.UsersTb.Include(u => u.Role)
                                                                                        .Where(u => u.Role.RoleName.Contains(rolename))
                                                                                        .ToListAsync();

        public new async Task<UsersTb> Get(string id) => await _appContext.UsersTb.Include(u => u.Role)
                                                                            .FirstOrDefaultAsync(u => u.UserId.ToString() == id);

        public async Task<UsersTb> ChangeActiveState(string id)
        {
            var activeUser = await _appContext.UsersTb.Include(u => u.Role)
                             .FirstOrDefaultAsync(u => u.UserId.ToString() == id);
            activeUser.IsEnabled = !activeUser.IsEnabled;
            _appContext.UsersTb.Update(activeUser);
            await _appContext.SaveChangesAsync();
            return activeUser;
        }
        //.ToListAsync();


    }
}
