using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class EditEmployeeDto: BaseSaveEmployeeDto
    {
        [Required]
        public int Id { get; set; }
    }
}
