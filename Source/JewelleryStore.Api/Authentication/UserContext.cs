using JewelleryStore.Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace JewelleryStore.Api.Authentication
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserRno => Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public string UserId => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
    }
}
