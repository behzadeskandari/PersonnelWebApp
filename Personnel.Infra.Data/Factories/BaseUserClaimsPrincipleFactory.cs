using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Personnel.Application.Interfaces;
using Personnel.Application.Mangers;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Factories
{

    public class BaseUserClaimsPrincipleFactory : UserClaimsPrincipalFactory<User, Roles>
    {
        private readonly IServiceProvider _serviceProvider;

        public BaseUserClaimsPrincipleFactory(BaseUserManager userManager, BaseRoleManager roleManager, IOptions<IdentityOptions> options, IServiceProvider serviceProvider) : base(userManager, roleManager, options)
        {
            _serviceProvider = serviceProvider;
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var userRoles = (await userService.GetUserRoles(user.Id)).Select(x => x.SystemName).ToList();
            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity = await base.GenerateClaimsAsync(user);


            //claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString(),ClaimValueTypes.Integer));
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Email,user?.Email));
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Actor, user.FullName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
            claimsIdentity.AddClaim(new Claim("Location", user.UserLocationId != null ? user.UserLocationId.Value.ToString() : "0", ClaimValueTypes.Integer));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, user.NationalCode));
            claimsIdentity.AddClaim(new Claim("OperationUnitCode", user.OperationUnitCode));

            // claimsIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone,user.PhoneNumber));
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData,user.GeneratedCode));

            foreach (var roles in userRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roles));
            }

            if (userRoles.All(x => x != "Administrator"))
            {
                var userPermissions = (await userService.GetUserPermissions(user.Id)).ToList();

                foreach (var item in userPermissions)
                {
                    claimsIdentity.AddClaim(new Claim(PersonnelClaimType.Permission, item.SystemName));
                }
            }


            return claimsIdentity;
        }
    }
}
