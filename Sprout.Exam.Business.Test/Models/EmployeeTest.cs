using Sprout.Exam.Business.Models;
using Xunit;

namespace Sprout.Exam.Business.Test.Models
{
    public class EmployeeTest
    {
        [Fact]
        public void EmployeeSetFullName()
        {
            // Arrange
            Employee employee = new()
            {
                // Act
                FullName = "Juan Dela Cruz"
            };

            // Assert
            Assert.Equal("Juan Dela Cruz", employee.FullName);
        } 
    }
}