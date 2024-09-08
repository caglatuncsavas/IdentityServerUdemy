using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UdemyIdentityServer.AuthServer.Repository;

namespace UdemyIdentityServer.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserRepository _customUserRepository;

        public CustomProfileService(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();

            var user = await _customUserRepository.FindById(int.Parse(subId));

            // We will be able to get the user's email or other information from the claim without going to the database.

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("city", user.City)
            };

            if(user.Id == 1)
            {
                claims.Add(new Claim("role", "admin"));
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claims);

           // context.IssuedClaims = claims; // Jwt içinde görmek istiyorsanız bu property'i set etmeniz gerekiyor.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();

            var user = await _customUserRepository.FindById(int.Parse(userId));

            context.IsActive = user != null ? true : false;
        }
    }
}
