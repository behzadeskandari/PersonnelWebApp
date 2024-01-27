using Microsoft.AspNetCore.Http;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Personnel.Domain.Extensions;

namespace Personnel.Application.Interfaces
{
    public partial class WebWorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _currentUser = null;
        public WebWorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual User CurrentUser => _currentUser;


        public virtual async Task SetCurrentUser()
        {
            if (_currentUser != null)
                await Task.CompletedTask;
            if (_httpContextAccessor.HttpContext?.User.Identity is
                {
                    IsAuthenticated: true
                })
            {
                var user = new User
                {
                    Id = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UserName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name),
                    Email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email),
                    FirstName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName),
                    LastName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Surname),
                    NationalCode = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData),
                    OperationUnitCode = _httpContextAccessor.HttpContext.User.FindFirstValue("OperationUnitCode"),


                };
                var location = _httpContextAccessor.HttpContext.User.FindFirstValue("Location");
                if (location != null)
                {
                    user.UserLocationId = int.Parse(location);
                }
                _currentUser = user;

            }

            await Task.CompletedTask;

        }

        public async Task SetBlazorUser(AuthenticationStateProvider authenticationStateProvider)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var dbUser = new User
            {
                Id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)),
                UserName = user.FindFirstValue(ClaimTypes.Name),
                Email = user.FindFirstValue(ClaimTypes.Email),
                FirstName = user.FindFirstValue(ClaimTypes.GivenName),
                LastName = user.FindFirstValue(ClaimTypes.Surname),
                NationalCode = user.FindFirstValue(ClaimTypes.UserData),
                OperationUnitCode = user.FindFirstValue("OperationUnitCode"),


            };
            var location = user.FindFirstValue("Location");
            if (location != null)
            {
                dbUser.UserLocationId = int.Parse(location);
            }
            _currentUser = dbUser;
        }
    }

}
