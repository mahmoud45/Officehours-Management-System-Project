using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public class OfficeHourseService : IOfficeHourseService
    {
        private readonly AppDbContext _context;
        public OfficeHourseService(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IEnumerable<OfficeHour>> GetStaffOfficeHoursAsync(string staffName)
        {
            var staff = await _context.Users.AsNoTracking()
            .Where(u => u.UserName.Equals(staffName))
            .FirstOrDefaultAsync();

            var officeHours = await _context.OfficeHours
            .AsNoTracking()
            .Where(o => o.StaffId.Equals(staff.Id))
            .ToListAsync();

            return officeHours;
        }

        public async Task Create(OfficeHour officeHour, string staffName)
        {
            var staff = await _context.Users.AsNoTracking().Where(u => u.UserName.Equals(staffName)).FirstOrDefaultAsync();

            if(staff == null)
                throw new System.Exception("Staff does not exist");
            
            OfficeHour _officeHour = new OfficeHour
            {
                Day = officeHour.Day,
                StartHour = officeHour.StartHour,
                EndHour = officeHour.EndHour,
                StaffId = staff.Id
            };

            await _context.OfficeHours.AddAsync(_officeHour);

            await _context.SaveChangesAsync();
        }

        public async Task<OfficeHour> GetOfficeHourByIdAsync(int Id) => await _context.OfficeHours.FindAsync(Id);

        public async Task Delete(int Id, string staffName)
        {
            var staff = await _context.Users.Where(u => u.UserName.Equals(staffName)).FirstOrDefaultAsync();
            var officeHour = await _context.OfficeHours.Where(o => o.Id == Id).Include(o => o.Staff).FirstOrDefaultAsync();

            if(officeHour == null || staff == null)
                throw new System.Exception("Officehour or staff does not exist.");
            
            if(!staff.Id.Equals(officeHour.StaffId))
                throw new System.Exception("You are not authorized");

            _context.Remove(officeHour);

            await _context.SaveChangesAsync();
        }
    }
}