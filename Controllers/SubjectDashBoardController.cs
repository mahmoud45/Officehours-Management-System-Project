using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using officehours_management.Data.Services;
using officehours_management.Data.Static;
using officehours_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace officehours_management.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = UserRoles.AdminAndSuperUser)]
    public class SubjectDashBoardController : Controller
    {
        private readonly IDashBoardService _service;
        public SubjectDashBoardController(IDashBoardService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Subjects(int? page, string SearchString)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);

            if(!string.IsNullOrEmpty(SearchString))
            {
                pageNumber = 1;
                var result = await _service.GetSubjectByNameAsync(SearchString);
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            var subjects = await _service.GetSubjectsAsync();
            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            return View(new Subject());
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name")]Subject subject)
        {
            var checkSubject = await _service.GetSubjectByNameAsync(subject.Name, exactMatch: true);

            if(checkSubject.Any())
            {
                TempData["Error"] = "Subject with the same name already exist.";
                return View(subject);
            }

            if(ModelState.IsValid)
            {
                await _service.CreateSubjectAsync(subject);
                return RedirectToAction("Subjects");
            }

            return View(subject);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var subject = await _service.GetSubjectByIdAsync(Id);

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int Id)
        {
            var subject = await _service.GetSubjectByIdAsync(Id);
            try{
                await _service.DeleteSubjectByIdAsync(Id);
            }catch(Exception ex){
                TempData["Error"] = ex.Message;
                return View(subject);
            }
            return RedirectToAction("Subjects");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var subject = await _service.GetSubjectByIdAsync(Id);

            if(subject == null)
            {
                TempData["Error"] = "Subject not exist.";
                return RedirectToAction("Subjects");
            }

            return View(subject);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit([Bind("Id,Name")]Subject subject)
        {
            var _subject = await _service.GetSubjectByIdAsync(subject.Id);

            if(!_subject.Name.Equals(subject.Name))
            {
                Console.WriteLine("from if.");
                try{
                    await _service.EditSubjectAsync(subject);
                }catch(Exception ex){
                    TempData["Error"] = ex.Message;
                    return View(subject);
                }
            }
            return RedirectToAction("Subjects");
        }

        public async Task<IActionResult> AssignStaff(int? page, string SearchString,int subjectId)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);

            var staff = await _service.GetAllStaff(subjectId);

            TempData["subjectId"] = subjectId;

            if(!string.IsNullOrEmpty(SearchString))
            {
                pageNumber = 1;
                var result = new Dictionary<ApplicationUser, bool>();

                foreach(var s in staff)
                {
                    if(s.Key.FullName.Contains(SearchString))
                        result.Add(s.Key, s.Value);
                }
            
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            
            return View(staff.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> AssignStaff(IFormCollection formCollection)
        {
            if(formCollection.Keys.Any())
            {
                var staffToAssign = new Dictionary<string, bool>();

                foreach(var Key in formCollection.Keys)
                {
                    if(!Key.Contains("subject") && !Key.Contains("token"))
                        staffToAssign.Add(Key, formCollection[Key].Contains("true"));
                }

                try{
                    await _service.AssignStaffToSubjectAsync(staffToAssign, Int32.Parse(formCollection["subjectId"]));
                }catch(Exception ex){
                    TempData["Error"] = ex.Message;
                }
            }

            return RedirectToAction("Subjects");
        }
    }
}