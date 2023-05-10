using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace officehours_management.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="Full Name")]
        public string FullName { get; set; }
        
        [Display(Name="Active")]
        public bool? is_active { get; set; }

        [Display(Name="Staff")]
        public bool? is_staff { get; set; }

        [Display(Name="superuser")]
        public bool? is_superuser { get; set; }

        public ICollection<SubjectStaff> Subjects_Staff { get; set; }
        public ICollection<SubjectStudent> Subjects_student { get; set; }
        public ICollection<OfficeHour> officehours { get; set; }
    }
}