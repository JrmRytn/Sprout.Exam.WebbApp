using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprout.Exam.DataAccess.Entities
{
    public class EmployeeEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        [Required]
        public string FullName { get; set; } 
        [Required] 
        public DateTime Birthdate { get; set; } 
        [Required]
        public string Tin { get; set; }
        [Required]
        public int TypeId { get; set; } 
    }
}