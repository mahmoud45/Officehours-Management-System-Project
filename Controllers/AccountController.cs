using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using officehours_management.Data.Static;
using officehours_management.Data.ViewModels;
using officehours_management.Models;
using System;
using System.Linq;

namespace officehours_management.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            bool _IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            if(_IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View(new RegisterVM());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            bool _IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            if(_IsAuthenticated)
                return RedirectToAction("Index", "Home");
            if(!ModelState.IsValid)
                return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if(user != null)
            {
                TempData["Error"] = "This email already exist";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.Email,
                UserName = registerVM.UserName
            };
            newUser.is_active = true;
            var userResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            if(userResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            else{
                TempData["Error"] = userResponse.Errors.GetEnumerator().Current.Description;
                return View(registerVM);
            }
            await _singInManager.SignInAsync(newUser, false);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            string ReturnUrl = Request.HttpContext.Request.Query["ReturnUrl"];

            bool _IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            if(_IsAuthenticated)
                return RedirectToAction("Index", "Home");
            TempData["ReturnUrl"] = ReturnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            bool _IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            if(_IsAuthenticated)
                return RedirectToAction("Index", "Home");
            
            if(!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.Email) ?? null;
            if(user != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if(checkPassword)
                {
                    if((bool)!user.is_active)
                    {
                        TempData["Error"] = "This account is banned";
                        return View(loginVM);
                    }

                    var SignInResponse = await _singInManager.PasswordSignInAsync(user, loginVM.Password, false,false);
                    if(SignInResponse.Succeeded)
                    {
                        string ReturnUrl = Request.Form["ReturnUrl"];
                        if(!string.IsNullOrEmpty(ReturnUrl))
                            return Redirect(ReturnUrl);
                        
                        if(await _userManager.IsInRoleAsync(user, UserRoles.Student))
                            return RedirectToAction("Index", "Student", new {username = user.UserName});
                        else if(await _userManager.IsInRoleAsync(user, UserRoles.TeacherAssistant)
                            || await _userManager.IsInRoleAsync(user, UserRoles.Doctor))
                            return RedirectToAction("Index", "TeachingStaff", new {staffName = user.UserName});

                        return RedirectToAction("Index", "Home");
                    }                   
                }
                TempData["Error"] = "Wrong password.";
                return View(loginVM);
            }
            TempData["Error"] = "Invalide email.";
            return View(loginVM);
        }

        public async Task<IActionResult> SignOutAsync()
        {
            bool _IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            if(_IsAuthenticated)
                await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditProfile(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            return View(user);
        }

        [HttpPost, ActionName("EditProfile")]
        public async Task<JsonResult> EditProfileConfirm([FromBody]ApplicationUser userData)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userData.Email);

                user.FullName = userData.FullName;
                user.UserName = userData.UserName;

                var response = await _userManager.UpdateAsync(user);
                if(response.Succeeded)
                    return Json(new {success = true});
                else{
                    var errors = "";
                    if(response.Errors.Any())
                    {
                        foreach(var item in response.Errors)
                            errors += item.Description + "\n";
                    }
                    else{
                        return Json(new {success = false, error = "Faild to save data."});
                    }
                    return Json(new {success = false, error = errors});
                }
            }
            string error = "Faild to save data.";
            return Json(new {success = false, error = error});
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            bool _IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            if(!_IsAuthenticated)
                return RedirectToAction(nameof(Login));
            
            return View();
        }
    }
}