using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace officehours_management.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public ICollection<SubjectStaff> Subjects_Staff { get; set; }
        public ICollection<SubjectStudent> Subjects_student { get; set; }
        
    }
}