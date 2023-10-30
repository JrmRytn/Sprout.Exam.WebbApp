using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class CalculateDto
{
    [Required]
    public decimal? AbsentDays { get; set; }
    [Required]
    public decimal? WorkedDays { get; set; }
    [Required]
    public decimal? WorkedHours { get; set; }
} 
}
