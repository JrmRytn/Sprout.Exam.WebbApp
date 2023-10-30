namespace Sprout.Exam.Business.Test.DataTransferObjects
{
    public class CalculateDtoTest
    {
        [Fact]
        public void CalculateDto_Required_AbsentDays()
        {
            //Arange
            var dto = new CalculateDto();

            //Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, result => result.MemberNames.Contains("AbsentDays") && result.ErrorMessage == "The AbsentDays field is required.");

        }
        [Fact]
        public void CalculateDto_Required_WorkedDays()
        {
            //Arange
            var dto = new CalculateDto();

            //Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, result => result.MemberNames.Contains("WorkedDays") && result.ErrorMessage == "The WorkedDays field is required.");

        }
        [Fact]
        public void CalculateDto_Required_WorkedHours()
        {
            //Arange
            var dto = new CalculateDto();

            //Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, result => result.MemberNames.Contains("WorkedHours") && result.ErrorMessage == "The WorkedHours field is required.");

        }
    }
}