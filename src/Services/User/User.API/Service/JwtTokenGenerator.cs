
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.API.Models;
using User.API.Service.IService;
using static User.API.Enums.Authorization;

namespace User.API.Service
{
    public class JwtTokenGenerator
     
        : IJwtTokenGenerator
    {

        private readonly JWTConfig _jwtConfig;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenGenerator(IOptions<JWTConfig> jwtOptions, UserManager<ApplicationUser> userManager)
        {
            _jwtConfig = jwtOptions.Value;
            _userManager = userManager;
        }

        public async  Task<string> CreateJwtToken(ApplicationUser applicationUser, Double expire)
        {
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email!),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName !)
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(expire), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
