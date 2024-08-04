using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] Register model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.RegisterAsync(model);
            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        
        [Route("token")]
        [HttpGet]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.GetTokenAsync(model);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [Route("assignRole")]
        [HttpPost]
        //this is important line that make error
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AssignRoleAsync([FromBody] AssignRole model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.AssignRoleAsync(model);

            if (!result.IsNullOrEmpty())
                return BadRequest(result);
            return Ok(model);
            
        }
    }
}
