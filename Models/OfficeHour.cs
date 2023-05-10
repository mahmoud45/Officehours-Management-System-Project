using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace officehours_management.Models
{
    public class OfficeHour
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(45)")]
        [Required(ErrorMessage = "Day field is required")]
        public string Day { get; set; }
        [Column(TypeName = "varchar(45)")]
        [Display(Name = "Start Hour")]
        [Required]
        [RegularExpression(@"^\d{2}:\d{2} ([a|A]|[p|P])[m|M]$", ErrorMessage = "This field must match this pattern(01:45 PM or 01:45 AM)")]
        public string StartHour { get; set; }
        [Column(TypeName = "varchar(45)")]
        [Display(Name = "End Hour")]
        [Required]
        [RegularExpression(@"^\d{2}:\d{2} ([a|A]|[p|P])[m|M]$", ErrorMessage = "This field must match this pattern(01:45 PM or 01:45 AM)")]
        public string EndHour { get; set; }
        public string StaffId { get; set; }
        [ForeignKey("StaffId")]
        public ApplicationUser Staff { get; set; }
    }
}