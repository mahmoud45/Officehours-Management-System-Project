using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using officehours_management.Data.Services;
using officehours_management.Models;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using officehours_management.Data.Static;

namespace officehours_management.Controllers
{
    [Authorize(Roles = UserRoles.TeacherAssistantAndDoctor)]
    public class TeachingStaffController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly ITeachingStaffService _teachingStaffService;

        public TeachingStaffController(ISubjectService subjectService,ITeachingStaffService teachingStaffService)
        {
            _subjectService = subjectService;
            _teachingStaffService = teachingStaffService;
        }

        public IActionResult Index(string staffName)
        {
            var currentUserName = User.Identity.Name;

            if(!currentUserName.Equals(staffName))
                return RedirectToAction("AccessDenied", "Account");
            
            return View();
        }

        public async Task<IActionResult> Subjects(int? page, string SearchString)
        {
            int pageSize = 2;
            int pageNumber = page ?? 1;
            if(!string.IsNullOrEmpty(SearchString))
            {
                pageNumber = 1;
                var result = await _subjectService.GetSubjectByNameAsync(SearchString);
                return View(result.ToPagedList(pageNumber, pageSize));
            }

            var subjects = await _subjectService.GetSubjectsAsync();
            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> TeachingStaffSubjects(int? page, string staffName)
        {
            if(!User.Identity.Name.Equals(staffName))
                return RedirectToAction("AccessDenied", "Account");
            
            int pageSize = 2;
            int pageNumber = page ?? 1;
            
            var subjects = await _subjectService.GetStaffSubjectsAsync(staffName);

            TempData["StaffName"] = staffName;
            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Staff(int? page, string SearchString)
        {
            if(!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("AccessDenied", "Account");

            int pageSize = 2;
            int pageNumber = page ?? 1;

            if(!string.IsNullOrEmpty(SearchString))
            {
                var staff= await _teachingStaffService.GetTeachingStaffByName(SearchString);

                return View(staff.ToPagedList(pageNumber, pageSize));
            }

            var _staff = await _teachingStaffService.GetTeachingStaffAsync();
            return View(_staff.ToPagedList(pageNumber, pageSize));
        }
    }
}