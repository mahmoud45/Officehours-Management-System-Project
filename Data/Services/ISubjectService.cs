using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task<IEnumerable<Subject>> GetSubjectByNameAsync(string SearchString, bool exactMatch=false);
        Task EnrollInSubjectAsync(int SubjectId, string UserId);
        Task <IEnumerable<Subject>> GetStudentSubjectsAsync(string studentName);
        Task <IEnumerable<Subject>> GetStaffSubjectsAsync(string staffName);
        
    }
}