using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using officehours_management.Data.Static;
using officehours_management.Models;

namespace officehours_management.Data.Services
{
    public class SubjectService: ISubjectService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubjectService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            var subjects = await _context.Subjects
            .Include(s => s.Subjects_Staff)
                .ThenInclude(ss => ss.Staff)
            .ToListAsync();

            return subjects;
        }

        public async Task<IEnumerable<Subject>> GetSubjectByNameAsync(string SearchString, bool exactMatch=false)
        {
            if(!exactMatch)
            {
                var subjects = await _context.Subjects
                    .Where(s => s.Name.Contains(SearchString))
                    .Include(s => s.Subjects_Staff)
                        .ThenInclude(ss => ss.Staff)
                    .ToListAsync();
                
                return subjects;
            }
            
            var exactSubject = await _context.Subjects
                .Where(s => s.Name.Equals(SearchString))
                .Include(s => s.Subjects_Staff)
                    .ThenInclude(ss => ss.Staff)
                .ToListAsync();

            return exactSubject;
        }

        public async Task EnrollInSubjectAsync(int SubjectId, string StudentId)
        {
            var subject = await _context.Subjects.Where(s => s.Id == SubjectId)
                .Include(s => s.Subjects_student)
                    .ThenInclude(ss => ss.Student)
                .FirstOrDefaultAsync();

            var student = await _context.Users.AsNoTracking().Where(u => u.Id.Equals(StudentId)).FirstOrDefaultAsync();

            if(subject == null || student == null)
                throw new Exception("Student of subject does not exist");
            
            var check = subject.Subjects_student.Where(ss => ss.StudentId.Equals(student.Id)).FirstOrDefault();
            if(check != null)
                throw new Exception($"You are already enrolled in {subject.Name}");

            var subjectStudent = new SubjectStudent
            {
                SubjectId = subject.Id,
                StudentId = StudentId
            };

            await _context.SubjectStudents.AddAsync(subjectStudent);

            await _context.SaveChangesAsync();
        }

        public async Task <IEnumerable<Subject>> GetStudentSubjectsAsync(string studentName)
        {
            var student = await _context.Users.AsNoTracking()
            .Where(u => u.UserName.Equals(studentName))
            .FirstOrDefaultAsync();
            
            var subjectStudent = await _context.SubjectStudents
            .Where(ss => ss.StudentId.Equals(student.Id))
            .Include(ss => ss.subject)
            .Select(ss => ss.SubjectId)
            .ToListAsync();

            var subjects = await _context.Subjects
            .Where(s => subjectStudent.Contains(s.Id))
            .Include(s => s.Subjects_Staff)
                .ThenInclude(ss => ss.Staff)
            .ToListAsync();

            return subjects;
        }

        public async Task<IEnumerable<Subject>> GetStaffSubjectsAsync(string staffName)
        {
            var staff = await _context.Users.AsNoTracking()
            .Where(s => s.UserName.Equals(staffName))
            .FirstOrDefaultAsync();

            var subjectStaff = await _context.SubjectStaff
            .Where(ss => ss.StaffId.Equals(staff.Id))
            .Include(ss => ss.subject)
            .Select(ss => ss.SubjectId)
            .ToListAsync();

            var subjects = await _context.Subjects
            .Where(s => subjectStaff.Contains(s.Id))
            .Include(s => s.Subjects_Staff)
                .ThenInclude(ss => ss.Staff)
            .ToListAsync();

            return subjects;
        }
    }
}