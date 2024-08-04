using Auth.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Services
{
    public interface IAuthService
    {
        Task<Authentication> RegisterAsync(Register model);
        Task<Authentication> GetTokenAsync(TokenRequest model);
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
        Task<string> AssignRoleAsync(AssignRole model);
    }
}
