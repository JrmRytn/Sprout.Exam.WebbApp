using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sprout.Exam.Business.Contracts;
using Sprout.Exam.WebApp.Controllers;
using Moq;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.Exceptions; 
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Sprout.Exam.WebApp.Test.Controllers
{
    public class EmployeeControllerTest
    {
        private readonly EmployeesController _controller; 
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly Mock<IMapper> _mapper;

        public EmployeeControllerTest()
        {
            _employeeRepository = new Mock<IEmployeeRepository>();
            _mapper = new Mock<IMapper>();

            _controller = new EmployeesController(_employeeRepository.Object, _mapper.Object);
        }
        private static List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>()
            {
                new() {Id = 1, FullName ="Juan Dela Cruz", Birthdate = "1996-03-29",Tin ="12345", TypeId = (EmployeeType)1 },
                new() {Id = 2, FullName ="Pedro Dela Cruz", Birthdate = "1996-03-29",Tin ="12346", TypeId = (EmployeeType)1 },
            };

            return employees;
        }
        [Fact]
        public async Task GetReturnsOkResult()
        {
            // Arrange
            _employeeRepository.Setup(x => x.GetAll()).ReturnsAsync(GetAllEmployees());
            
            // Act
            var getResult = await _controller.Get();

            // Assert
            var assertResult = Assert.IsType<OkObjectResult>(getResult);
            var employee = Assert.IsAssignableFrom<IEnumerable<Employee>>(assertResult.Value);
            Assert.Equal(2,employee.Count());
        }


        [Fact]
        public async Task GetByIdReturnsOkResult()
        {
            // Arrange 
            int employeeId = 1; 
            var employeeToReturn = GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);  

            _employeeRepository.Setup(x => x.GetById(employeeId)).ReturnsAsync(employeeToReturn);

            var employeeDtoToReturn = new EmployeeDto
            { 
                Id = employeeToReturn.Id,
                FullName = employeeToReturn.FullName,
                Birthdate = employeeToReturn.Birthdate,
                Tin = employeeToReturn.Tin,
                TypeId = (short)employeeToReturn.TypeId
            }; 
            _mapper.Setup(m => m.Map<EmployeeDto>(employeeToReturn)).Returns(employeeDtoToReturn); 
            // Act
            var result = await _controller.GetById(employeeId);

            // Assert
            var assertResult = Assert.IsType<OkObjectResult>(result);
            var employee = Assert.IsType<EmployeeDto>(assertResult.Value);
            
            Assert.Equal("Juan Dela Cruz", employee.FullName);
            Assert.Equal(1,employee.Id);
            Assert.Equal("1996-03-29",employee.Birthdate);
            Assert.Equal("12345",employee.Tin);
            Assert.Equal(1,(short)employee.TypeId);
        }
        [Fact]
        public async Task PutReturnOkResult()
        {
            // Arrange 
            int employeeId = 1; 
            var employeeToReturn = GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);  
            var updatedEmployee = new Employee()
            {
                Id = employeeId,
                FullName = "Juan Dela Cruz 2",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId = (EmployeeType)2
            };

            _employeeRepository.Setup(x => x.Update(employeeId,updatedEmployee)).Returns(Task.CompletedTask);
            
            var editEmployee = new Employee
            { 
                Id = updatedEmployee.Id,
                FullName = updatedEmployee.FullName,
                Birthdate = updatedEmployee.Birthdate,
                Tin = updatedEmployee.Tin,
                TypeId = (EmployeeType)2
            }; 
            
            var editEmployeeDto = new EditEmployeeDto
            { 
                Id = updatedEmployee.Id,
                FullName = updatedEmployee.FullName,
                Birthdate = updatedEmployee.Birthdate,
                Tin = updatedEmployee.Tin,
                TypeId = (short)updatedEmployee.TypeId
            }; 
            
            _mapper.Setup(m => m.Map<Employee>(editEmployeeDto)).Returns(editEmployee);
            // Act
            var result = await _controller.Put(editEmployeeDto); 
            
            // Assert
            var assertResult = Assert.IsType<OkObjectResult>(result);
            var employee = Assert.IsType<Employee>(assertResult.Value);
            
            Assert.Equal("Juan Dela Cruz 2", employee.FullName);
            Assert.Equal(1,employee.Id);
            Assert.Equal("1996-03-29",employee.Birthdate);
            Assert.Equal("12345",employee.Tin);
            Assert.Equal(2,(short)employee.TypeId); 
        } 
        [Fact]
        public async Task PutReturnNotFoundResult()
        {
             // Arrange
            var employeeDto = new EditEmployeeDto 
            {  
                Id = 3,
                FullName ="Juan Dela Cruz",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId = 2
            };

            _employeeRepository.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<Employee>()))
                .Throws(new NotFoundException("Employee not found"));
            var editEmployee = new Employee
            { 
                Id = 3,
                FullName ="Juan Dela Cruz",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId = (EmployeeType)2
            }; 
            _mapper.Setup(m => m.Map<Employee>(employeeDto)).Returns(editEmployee);
            // Act
            var result = await _controller.Put(employeeDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found", notFoundResult.Value);
        }

        [Fact]
        public async Task PostReturnOkResult()
        {
            // Arrange
            var createEmployeeDto = new CreateEmployeeDto()
            { 
                FullName ="Juan Dela Cruz",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId =2
            };

            var createEmployee = new Employee()
            { 
                Id = 1,
                FullName ="Juan Dela Cruz",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId =(EmployeeType) 2
            };
            _employeeRepository.Setup(x => x.Create(createEmployee)).ReturnsAsync(createEmployee.Id);
            _mapper.Setup(m => m.Map<Employee>(createEmployeeDto)).Returns(createEmployee);

            // Act 
            var result = await _controller.Post(createEmployeeDto);

            // Assert
            var assert = Assert.IsType<CreatedResult>(result);
            var employeeId = Assert.IsType<int>(assert.Value);
            Assert.NotEqual(0,employeeId);
        }
        [Fact]
        public async Task CalculateRegularEmployeeWithOneAbsentReturnOkResult()
        {
            // Arrange 

            var employee = new Employee()
            { 
                Id = 1,
                FullName ="Juan Dela Cruz",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId =(EmployeeType) 1
            };
            _employeeRepository.Setup(x => x.GetById(employee.Id)).ReturnsAsync(employee); 
            var calculateDto = new CalculateDto()
            {
                AbsentDays = 1,
                WorkedDays = 0,
                WorkedHours = 0
            };
            // Act 
            var result = await _controller.Calculate(employee.Id,calculateDto);
            
            // Assert
            var assert = Assert.IsType< OkObjectResult>(result);
            var salary = Assert.IsType<double>(assert.Value);
            Assert.Equal(16690.91,salary);
        }
        [Fact]
        public async Task CalculateContractualEmployeeReturnOkResult()
        {
            // Arrange 

            var employee = new Employee()
            { 
                Id = 1,
                FullName ="Juan Dela Cruz",
                Birthdate = "1996-03-29",
                Tin = "12345",
                TypeId =(EmployeeType) 2
            };
            _employeeRepository.Setup(x => x.GetById(employee.Id)).ReturnsAsync(employee); 
            var calculateDto = new CalculateDto()
            {
                AbsentDays = 0,
                WorkedDays = 15.5m,
                WorkedHours = 0
            };
            // Act 
            var result = await _controller.Calculate(employee.Id,calculateDto);
            
            // Assert
            var assert = Assert.IsType< OkObjectResult>(result);
            var salary = Assert.IsType<double>(assert.Value);
            Assert.Equal(7750.00,salary);
        }
    }
}