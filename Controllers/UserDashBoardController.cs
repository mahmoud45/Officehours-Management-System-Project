using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using officehours_management.Data.Static;
using officehours_management.Data.Services;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace officehours_management.Controllers
{
    [Authorize(Roles = UserRoles.AdminAndSuperUser)]
    public class UserDashBoardController : Controller
    {
        private readonly IDashBoardService _service;

        public UserDashBoardController(IDashBoardService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Users(int? page, string SearchString)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if(!string.IsNullOrEmpty(SearchString))
            {
                pageNumber = 1;
                var result = await _service.SearchUserByName(SearchString);
                return View(result.ToPagedList(pageNumber, pageSize));
            }

            var Users = await _service.GetUsersAsync();

            return View(Users.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> ManageUserAccount(string UserName)
        {
            var user = await _service.GetUserByUserName(UserName);

            var roles = await _service.GetAllRoles();

            TempData["AllRoles"] = roles;
            
            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> ManageUserAccount(IFormCollection formCollection)
        {
            Dictionary<string, bool> RolesDict = new Dictionary<string, bool>();

            var allRoles = await _service.GetAllRoles();

            foreach(string role in allRoles)
            {
                if(!string.IsNullOrEmpty(formCollection[role]))
                    RolesDict.Add(role, formCollection[role].Contains("true"));
            }

            RolesDict.Add("is_active", formCollection["is_active"].Contains("true"));
            
            var user = await _service.GetUserByUserName(formCollection["UserName"]);

            TempData["AllRoles"] = allRoles;

            foreach(var (key, value) in RolesDict)
            {
                if(value)
                {
                    try{
                        await _service.AddToRoleAsync(user, key);
                    }catch(Exception ex){
                        TempData["error"] = ex.Message;
                        return View(user);
                    }
                }else{
                    try{
                        await _service.RemoveFromRoleAsync(user, key);
                    }catch(Exception ex){
                        TempData["error"] = ex.Message;
                        return View(user);
                    }
                }
            }
            return RedirectToAction("Users", new {page=1, SearchString=""});
        }
    }
}