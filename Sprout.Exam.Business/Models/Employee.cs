using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Models
{
    public class Employee : Salary
    {
        public Employee()
        {
            
        }
        public Employee(int id) : base()
        {
            Id = id;
        }

        public Employee(int id, string fullName, string birthdate, string tin, int typeId) : this(id)
        {
            FullName = fullName;
            Birthdate = birthdate;
            Tin = tin;
            TypeId = (EmployeeType)typeId;
        }

        public int Id { get; set; } 
        public string FullName { get; set; } 
        public string Birthdate { get; set; } 
        public string Tin { get; set; } 
    }
}