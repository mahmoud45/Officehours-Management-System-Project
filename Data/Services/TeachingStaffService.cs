using System.Threading.Tasks;
using System.Collections.Generic;
using officehours_management.Models;
using Microsoft.AspNetCore.Identity;
using officehours_management.Data.Static;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace officehours_management.Data.Services
{
    public class TeachingStaffService : ITeachingStaffService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TeachingStaffService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable<ApplicationUser>> GetTeachingStaffAsync()
        {
            var TeacherAssistants = await _userManager.GetUsersInRoleAsync(UserRoles.TeacherAssistant);
            var Doctors = await _userManager.GetUsersInRoleAsync(UserRoles.Doctor);

            var teachingStaff = TeacherAssistants
            .Concat(Doctors)
            .OfType<ApplicationUser>();
            
            return teachingStaff;
        }

        public async Task<IEnumerable<ApplicationUser>> GetTeachingStaffByName(string staffName)
        {
            var staff = await _context.Users
            .Where(u => u.FullName.Contains(staffName))
            .Include(s => s.officehours)
            .ToListAsync();

            return staff;
        }
    }
}