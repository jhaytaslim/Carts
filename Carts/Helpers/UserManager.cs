using Carts.Data;
using Carts.Data.POCO;
using Carts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Carts.Helpers
{

    public interface IUserManager
    {
        Task<bool> SignIn(HttpContext httpContext, LoginViewModel user, bool isPersistent = false);
        void SignOut(HttpContext httpContext);
        string GetCurrentUserId(HttpContext httpContext);
        Task<UsersTb> GetCurrentUser(HttpContext httpContext);
        Task<UsersTb> Create(UsersTb user);
        IEnumerable<Claim> GetUserClaims(UsersTb user);
    }

    public class UserManager : IUserManager
    {
        private IUnitOfWork _unitOfWork;
        private ILogger<dynamic> _log;

        public UserManager(IUnitOfWork unitOfWork, ILogger<dynamic> log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public async Task<bool> SignIn(HttpContext httpContext, LoginViewModel user, bool isPersistent = false)
        {
            try
            {
                var result = await _unitOfWork.User.SignIn(user.username, user.password);
                if (result)
                {
                    var dbUserData = await _unitOfWork.User.GetUserByUsername(user.username);

                    ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(dbUserData), CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    var authProperties = new AuthenticationProperties
                    {
                        // Refreshing the authentication session should be allowed.
                        AllowRefresh = true,

                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),

                        // The time at which the authentication ticket was issued.
                        IssuedUtc = DateTime.Now,

                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                        RedirectUri = "/home"


                    };

                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                    return true;
                }
            }
            catch(Exception ex)
            {
                _log.LogInformation(ex.Message+"\n"+ex.InnerException);
            }
           
            return false;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        public IEnumerable<Claim> GetUserClaims(UsersTb user)
        {
            try
            {
                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.FullName));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Role, user.Role?.RoleName));
                return claims;
            }
            catch(Exception ex)
            {
                _log.LogInformation(ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public string GetCurrentUserId(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return "-1";// -1;

            Claim claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
                return "-1";// - 1;

            string currentUserId;

            if(!string.IsNullOrEmpty(claim.Value.ToString()))
                currentUserId = claim.Value.ToString();
            else
                return "-1"; //- 1;

            return currentUserId;
        }

        public async Task<UsersTb> GetCurrentUser(HttpContext httpContext)
        {
            try
            {
                string currentUserId = this.GetCurrentUserId(httpContext);

                if (currentUserId == "-1" /*-1*/)
                    return null;

                return await _unitOfWork.User.Get(currentUserId);
            }
            catch(Exception ex)
            {
                _log.LogInformation(ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<UsersTb> Create(UsersTb user)
        {
            if (await _unitOfWork.User.AddAsync(user) != null)
            {
                return user;
            }

            return null;
        }

    }
}
