using System;
using Microsoft.AspNetCore.Http;

namespace ClientAPI.test.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string LoadPath()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            return "";
            //return _httpContextAccessor.HttpContext.Session.GetString();
        }
    }
}
