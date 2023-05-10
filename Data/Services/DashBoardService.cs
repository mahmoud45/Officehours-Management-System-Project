using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using officehours_management.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using officehours_management.Data.Static;

namespace officehours_management.Data.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DashBoardService(AppDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region User

        public async Task<List<string>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(role => role.Name).ToListAsync();
            return roles;
        }
        public async Task<ICollection<ApplicationUser>> SearchUserByName(string SearchString)
        {
            var users = await _context.Users.AsNoTracking().Where(u => u.FullName.Contains(SearchString)).ToListAsync();
            return users;
        }

        public async Task<ICollection<ApplicationUser>> GetUsersAsync()
        {
            var users = await _context.Users.Where(u => u.is_superuser != true).AsNoTracking().ToListAsync();
            return users;
        }

        public async Task<ApplicationUser> GetUserByUserName(string UserName) => await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(UserName));
    
        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            if(role.Equals("is_active"))
            {
                user.is_active = true;
                _context.Entry(user).State = EntityState.Modified;
            }else if(role.Equals("SuperUser"))
            {
                throw new Exception("You are not authorized for this action.");
            }
            else if(!await _userManager.IsInRoleAsync(user, role))
            {
                var response = await _userManager.AddToRoleAsync(user, role);
                return response;
            }

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            if(role.Equals("is_active"))
            {
                user.is_active = false;
                _context.Entry(user).State = EntityState.Modified;
            }else if(role.Equals("SuperUser"))
            {
                throw new Exception("You are not authorized for this action.");
            }
            else if(await _userManager.IsInRoleAsync(user, role))
            {
                var response = await _userManager.RemoveFromRoleAsync(user, role);
                return response;
            }

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<Dictionary<ApplicationUser, bool>> GetAllStaff(int subjectId)
        {
            var TAs = await _userManager.GetUsersInRoleAsync(UserRoles.TeacherAssistant);
            var Doctors = await _userManager.GetUsersInRoleAsync(UserRoles.Doctor);
            
            var subject_staff = await _context.SubjectStaff.Where(ss => ss.SubjectId == subjectId).Select(s => s.StaffId).ToListAsync();

            var staff = TAs.Concat(Doctors).OfType<ApplicationUser>();

            //dictionary for aleardy staff of the subject in hand
            Dictionary<ApplicationUser, bool> _subject_staff= new Dictionary<ApplicationUser, bool>();

            foreach(ApplicationUser u in staff)
            {
                _subject_staff.Add(u, subject_staff.Contains(u.Id));
            }

            return _subject_staff;
        }
        #endregion User


        #region Subject

        public async Task<IEnumerable<Subject>> GetSubjectsAsync(){
            var subjects = await _context.Subjects
            .Include(subject => subject.Subjects_Staff)
                .ThenInclude(subject_staff => subject_staff.Staff)
            .ToListAsync();

            return subjects;
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return subject;
        }

        public async Task<IEnumerable<Subject>> GetSubjectByNameAsync(string searchString, bool exactMatch=false)
        {
            if(exactMatch)
            {
                var subjects = await _context.Subjects
                .Where(s => s.Name.Equals(searchString))
                .Include(subject => subject.Subjects_Staff)
                    .ThenInclude(subject_staff => subject_staff.Staff)
                .ToListAsync();

                return subjects;
            }
            var result = await _context.Subjects
            .Where(s => s.Name.Contains(searchString))
            .Include(subject => subject.Subjects_Staff)
                .ThenInclude(subject_staff => subject_staff.Staff)
            .ToListAsync();

            return result;
        }

        public async Task CreateSubjectAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if(subject != null)
            {
                _context.Subjects.Remove(subject);
            }else{
                throw new Exception("Subject does not exist.");
            }
            await _context.SaveChangesAsync();
        }

        public async Task EditSubjectAsync(Subject subject)
        {
            
            var checkSubject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Name.Equals(subject.Name));

            if(checkSubject != null)
                throw new Exception("Subject with the same name already exist.");

            checkSubject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Id == subject.Id);

            _context.Entry(checkSubject).CurrentValues.SetValues(subject);
            _context.Entry(checkSubject).Property(s => s.Name).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task AssignStaffToSubjectAsync(Dictionary<string, bool> staffToAssign, int subjectId)
        {
            var _SubjectStaff = await _context.SubjectStaff.Where(s => s.SubjectId == subjectId).Include(ss => ss.Staff).ToListAsync();
            
            var _subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Id == subjectId);
            
            if(_subject == null)
                throw new Exception("Subject not found.");

            foreach(var (staffId, assign) in staffToAssign)
            {
                var check = _SubjectStaff.Where(s => s.StaffId.Equals(staffId)).FirstOrDefault();
                if(assign)
                {
                    if(check != null)
                        throw new Exception($"{check.Staff.FullName} is already staff of this subject.");

                    var subject_staff = new SubjectStaff
                    {
                        StaffId = staffId,
                        SubjectId = subjectId
                    };

                    await _context.SubjectStaff.AddAsync(subject_staff);
                }else{
                    if(check != null)
                        _context.SubjectStaff.Remove(check);
                }
            }

            await _context.SaveChangesAsync();
        }

        #endregion Subject
    }
}