using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using officehours_management.Models;
using System.Threading.Tasks;
using officehours_management.Data.Services;
using Microsoft.AspNetCore.Authorization;
using officehours_management.Data.Static;
using X.PagedList;
using System.Linq;
using System;

namespace officehours_management.Controllers
{
    [Authorize(Roles = UserRoles.Student)]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubjectService _subjectService;
        private readonly IStudentService _studentService;
        public StudentController(UserManager<ApplicationUser> userManager, ISubjectService subjectService, IStudentService studentService)
        {
            _userManager = userManager;
            _subjectService = subjectService;
            _studentService = studentService;
        }

        public IActionResult Index(string username)
        {
            var currentUserName = User.Identity.Name;

            if(!username.Equals(currentUserName))
                return RedirectToAction("AccessDenied", "Account");

            return View();
        }

        public async Task<IActionResult> Subjects(int? page, string SearchString)
        {
            int pageSize = 1;
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

        public async Task<IActionResult> Enroll(string subjectName)
        {
            var subject = await _subjectService.GetSubjectByNameAsync(subjectName, true);

            if(!subject.Any())
            {
                TempData["Error"] = "Can't find subject";
                return RedirectToAction("Subjects");
            }

            return View(subject.FirstOrDefault());
        }

        [HttpPost, ActionName("Enroll")]
        public async Task<IActionResult> ConfirmEnroll(int SubjectId, string UserId)
        {
            try{
                await _subjectService.EnrollInSubjectAsync(SubjectId, UserId);
            }catch(Exception ex){
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Subjects));
            }

            return RedirectToAction("StudentSubjects", new {studentName = User.Identity.Name});
        }

        public async Task<IActionResult> StudentSubjects(int? page, string studentName)
        {
            if(!User.Identity.Name.Equals(studentName))
                return RedirectToAction("AccessDenied", "Account");

            int pageSize = 1;
            int pageNumber = page ?? 1;
            var subjectsStudent = await _subjectService.GetStudentSubjectsAsync(studentName);

            TempData["UserName"] = studentName;

            return View(subjectsStudent.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Students(int? page, string SearchString)
        {
            if(!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("AccessDenied", "Account");
                
            int pageSize = 1;
            int pageNumber = page ?? 1;

            var students = await _studentService.GetStudentsAsync();

            return View(students.ToPagedList(pageNumber, pageSize));
        }
    }
}