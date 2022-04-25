using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TravelManagementSystem.Models;

namespace TravelManagementSystem.Infrastructure.ApplicationUserClaims
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager
            , RoleManager<AppRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("UserName", user.UserName),
                    new Claim("Email",user.Email),
                    new Claim("Admin",user.NormalizedUserName),
                });
            }

            // You can add more properties that you want to expose on the User object below

            return principal;
        }
    }
}
