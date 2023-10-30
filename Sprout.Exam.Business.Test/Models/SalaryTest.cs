using Sprout.Exam.Business.Exceptions;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Common.Enums;
using Xunit;
namespace Sprout.Exam.Business.Test.Models
{
    public class SalaryTest
    {
        [Fact]
        public void CalculateRegularSalaryOneDayAbsent()
        {
            //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType)1
            };
            
            Salary salary = new((short)employee.TypeId); 

            //Act
            var regularSalary = salary.CalculateSalary(0,1,0);
            var expectredSalary = 16690.91;
            //Assert
            Assert.Equal(expectredSalary,regularSalary);
        }
        [Fact]
        public void CalculateRegularSalaryNoAbsent()
        {
            //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType)1
            };
            
            Salary salary = new((short)employee.TypeId); 

            //Act
            var regularSalary = salary.CalculateSalary(0,0,0);
            var expectredSalary = 17600.00;
            //Assert
            Assert.Equal(expectredSalary,regularSalary);
        }
        [Fact]
        public void CalculateContractualSalary()
        {
            //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType)2
            };
            
            Salary salary = new((short)employee.TypeId); 

            //Act
            var contractualSalary = salary.CalculateSalary(15.5m,0,0);
            var expectredSalary = 7750.00;
            //Assert
            Assert.Equal(expectredSalary,contractualSalary);
        }
        [Fact]
        public void CalculateProbationarySalaryOneDayAbsent()
        {
             //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType)3
            };
            
            Salary salary = new((short)employee.TypeId); 

            //Act
            var probationarySalary = salary.CalculateSalary(0,1,0);
            var expectredSalary = 15021.82;
            //Assert
            Assert.Equal(expectredSalary,probationarySalary);
        }
        [Fact]
        public void CalculateProbationarySalaryNoAbsent()
        {
             //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType)3
            };
            
            Salary salary = new((short)employee.TypeId); 

            //Act
            var probationarySalary = salary.CalculateSalary(0,0,0);
            var expectredSalary = 15840.00;
            //Assert
            Assert.Equal(expectredSalary,probationarySalary);
        }
        [Fact]
        public void CalculatePartTimeSalary()
        {
             //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType)4
            };
            
            Salary salary = new((short)employee.TypeId); 

            //Act
            var parttimeSalary = salary.CalculateSalary(0,0,3);
            var expectredSalary = 150.00;
            //Assert
            Assert.Equal(expectredSalary,parttimeSalary);
        }
        [Fact]
        public void CalculateSalaryInvalidEmployeeType()
        {
            //Arange
            Employee employee = new()
            {
                Id =1,
                FullName= "Juan Dela Cruz",
                Birthdate ="1996-03-23",
                Tin= "12345",
                TypeId =(EmployeeType) 5
            };
            
            Salary salary = new((short)employee.TypeId);  
            //Act and Asert
            //Throw NotFoundException if employee type id not exists.
            Assert.Throws<NotFoundException>(() => salary.CalculateSalary(0,0,0));
        }
    }
}