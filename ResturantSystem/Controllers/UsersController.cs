using Auth.Models;
using Auth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = await _userManager.Users.Select(user=> new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result,
                IsMine = user.Id == _userManager.GetUserId(HttpContext.User)
            }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null) return NotFound(); 
            
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if(userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user,role.RoleName);
                if(userRoles.Any(r => r != role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user,role.RoleName);
                if (userRoles.Count == 0 && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.Select(role => new RoleViewModel { 
                RoleId=role.Id,
                RoleName=role.Name
            }).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                Roles = roles
            };
            return View(viewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel userVM)
        {
            if (!ModelState.IsValid) return View(userVM);
            
            var user = new ApplicationUser
            {
                UserName = userVM.Username,
                Email = userVM.Email,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
            };
            var result = await _userManager.CreateAsync(user, userVM.Password);
            
            if(!result.Succeeded)
            {
                foreach(var error in  result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(userVM);
            }

            await _userManager.AddToRolesAsync(user, userVM.Roles.Where(r=>r.IsSelected).Select(r=>r.RoleName));

            return RedirectToAction(nameof(Index));
        }
        //remote attribute
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
                return Json(true);
            return Json(false);
        }
        public async Task<IActionResult> CheckUsername(string username)
        {
            if (await _userManager.FindByNameAsync(username) == null)
                return Json(true);
            return Json(false);
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();
            var userVM = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles.Select(r => new RoleViewModel
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };
            return View(userVM);
        }
        public async Task<IActionResult> CheckEmailInEdit(string email ,string id)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user != await _userManager.FindByIdAsync(id))
               return Json(false);
            return Json(true);
        }
        public async Task<IActionResult> CheckUsernameInEdit(string username ,string id)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && user != await _userManager.FindByIdAsync(id))
               return Json(false);
            return Json(true);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null) return NotFound();


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName= model.Username;

            await _userManager.UpdateAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                if (userRoles.Any(r => r != role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                if(userRoles.Count == 0 && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }
            return RedirectToAction(nameof(Index));
        }

       
    }
}
