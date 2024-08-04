using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UsersController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string userId)
        {

            if (userId == _userManager.GetUserId(HttpContext.User))
            {
                return Json(false);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }
            return Ok(true);
        }
        
    }
}


