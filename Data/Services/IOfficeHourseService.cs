using System.Collections.Generic;
using System.Threading.Tasks;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public interface IOfficeHourseService
    {
        Task <IEnumerable<OfficeHour>> GetStaffOfficeHoursAsync(string staffName);
        Task Create(OfficeHour officeHour, string staffName);
        Task<OfficeHour> GetOfficeHourByIdAsync(int Id);
        Task Delete(int Id, string staffName);
    }
}