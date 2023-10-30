using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Sprout.Exam.Business.DataTransferObjects;
using Xunit;

namespace Sprout.Exam.Business.Test.DataTransferObjects
{
    public class CreateEmployeeDtoTest
    {
        [Fact]
        public void CreateEmployeeDto_FullName_Required()
        {
            // Arrange
            var dto = new CreateEmployeeDto();

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, result => result.MemberNames.Contains("FullName") && result.ErrorMessage == "The FullName field is required.");
        }

        [Fact]
        public void CreateEmployeeDto_ValidData()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FullName = "John Doe",
                Tin = "12345",
                Birthdate = "2000-01-01",
                TypeId = 1
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults);

            // Assert
            Assert.True(isValid);
        }
        [Fact]
        public void CreateEmployeeDto_InvalidData()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FullName = null,
                Tin = null,
                Birthdate = null,
                TypeId = 0
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
        }
    }
}