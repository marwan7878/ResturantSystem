using Auth.Helpers;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<Authentication> RegisterAsync(Register model)
        {
            if(await _userManager.FindByEmailAsync(model.Email) != null)
                return new Authentication { Message = "Email is already taken!"};
            var user = new ApplicationUser 
            { 
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = new MailAddress(model.Email).User
            };

            var result = await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new Authentication { Message= errors};
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new Authentication
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }
        public async Task<Authentication> GetTokenAsync(TokenRequest model)
        {
            var authModel = new Authentication();

            var user = await _userManager.FindByEmailAsync(model.Email);
            user = await _userManager.FindByNameAsync(model.Email);
            


            if(user == null || !await _userManager.CheckPasswordAsync(user,model.Password))
            {
                authModel.Message = "Sorry, Email or Password is incorrect.";
                return authModel;
            }

            var jwt = await CreateJwtToken(user);
            
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
            authModel.ExpiresOn = jwt.ValidTo;
            authModel.Email = model.Email;
            authModel.Roles = (await _userManager.GetRolesAsync(user)).ToList();

            return authModel;
        }
        
        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            //claims is any info that i need to be in jwt

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles" , role));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid",user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer:_jwt.Issuer,
                audience:_jwt.Audience,
                claims:claims,
                expires:DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials:signingCredentials
            );

            return jwtSecurityToken;
        }
        public async Task<string> AssignRoleAsync(AssignRole model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var role = await _roleManager.FindByNameAsync(model.Role);
            if (user == null || role == null)
                return "Invalid User ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User is already assigned to this role!";

            var result = await _userManager.AddToRoleAsync(user,model.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

    }
}
