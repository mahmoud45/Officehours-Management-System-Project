using System.Threading.Tasks;
using System.Collections.Generic;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public interface ITeachingStaffService
    {
        Task<IEnumerable<ApplicationUser>> GetTeachingStaffAsync();
        Task<IEnumerable<ApplicationUser>> GetTeachingStaffByName(string staffName);
    }
}