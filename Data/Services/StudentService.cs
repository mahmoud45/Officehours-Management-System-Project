using Microsoft.AspNetCore.Identity;
using officehours_management.Data.Static;
using officehours_management.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace officehours_management.Data.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetStudentsAsync()
        {
            var students = await _userManager.GetUsersInRoleAsync(UserRoles.Student);
            students = students.Select(s => new ApplicationUser
            {
                Email = s.Email,
                FullName = s.FullName
            }).ToList();

            return students;
        }
    }
}