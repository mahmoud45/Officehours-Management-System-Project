using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using officehours_management.Data.Static;
using officehours_management.Data.Services;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using officehours_management.Models;

namespace officehours_management.Controllers
{
    [Authorize(Roles = UserRoles.TeacherAssistantAndDoctor)]
    public class OfficeHourController : Controller
    {
        private readonly IOfficeHourseService _officehoursService;
        public OfficeHourController(IOfficeHourseService officeHourseService)
        {
            _officehoursService = officeHourseService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page, string staffName)
        {
            if(!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("AccessDenied", "Account");

            int pageSize =1;
            int pageNumer = page ?? 1;

            TempData["staffName"] = staffName;

            var officeHours = await _officehoursService.GetStaffOfficeHoursAsync(staffName);
            
            return View(officeHours.ToPagedList(pageNumer, pageSize));
        }

        public IActionResult Create(string staffName)
        {
            var currentUserName = User.Identity.Name;

            if(!currentUserName.Equals(staffName))
                return RedirectToAction("AccessDenied", "Account");

            ViewBag.Day = new SelectList(OfficeHourDay.Day, "Value", "Text");

            return View(new OfficeHour());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string staffName, [Bind("Day", "StartHour", "EndHour")]OfficeHour officeHour)
        {
            var currentUserName = User.Identity.Name;

            if(!currentUserName.Equals(staffName))
                return RedirectToAction("AccessDenied", "Account");

            if(ModelState.IsValid)
            {
                await _officehoursService.Create(officeHour, staffName);
                return RedirectToAction("Index", new {staffName = staffName});
            }
            ViewBag.Day = new SelectList(OfficeHourDay.Day, "Value", "Text");
            return View(officeHour);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var officeHour = await _officehoursService.GetOfficeHourByIdAsync(Id);

            return View(officeHour);
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult>ConfirmDelete(int Id)
        {
            var currentStaffName = User.Identity.Name;

            try{
                await _officehoursService.Delete(Id, currentStaffName);
            }catch(System.Exception ex){
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index), new {staffName=currentStaffName});
        }
    }
}