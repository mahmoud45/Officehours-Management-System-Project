using System.Collections.Generic;
using System.Threading.Tasks;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<ApplicationUser>> GetStudentsAsync();
    }
}