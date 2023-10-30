using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public abstract class BaseSaveEmployeeDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Tin { get; set; }
        [Required] 
        public string Birthdate { get; set; }
        [Required(ErrorMessage ="Employee type is required.")] 
        public int TypeId { get; set; }
    }
}
