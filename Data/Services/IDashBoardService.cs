using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public interface IDashBoardService
    {
        Task<List<string>> GetAllRoles();

        #region User

        Task<ICollection<ApplicationUser>> GetUsersAsync();
        Task<ICollection<ApplicationUser>> SearchUserByName(string SearchString);
        Task<ApplicationUser> GetUserByUserName(string UserName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);

        Task<Dictionary<ApplicationUser, bool>> GetAllStaff(int subjectId);

        #endregion user

        #region Subject
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(int id);
        Task<IEnumerable<Subject>> GetSubjectByNameAsync(string searchString, bool exactMatch=false);
        Task DeleteSubjectByIdAsync(int id);
        Task CreateSubjectAsync(Subject subject);
        Task EditSubjectAsync(Subject subject);
        Task AssignStaffToSubjectAsync(Dictionary<string, bool> staffToAssign, int subjectId);

        #endregion Subject
    }
}