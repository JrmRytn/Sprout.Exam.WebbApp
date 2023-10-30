using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.Models;
using Sprout.Exam.DataAccess.Data;
using Sprout.Exam.DataAccess.Entities;
using Sprout.Exam.DataAccess.Repositories;
using Sprout.Exam.WebApp.Mapping;
using Xunit;

namespace Sprout.Exam.DataAccess.Test.Repositories
{
    public class EmployeeRepositoryTest
    {
        private readonly IMapper _mapper;
        private readonly DataDbContext _context;
        public EmployeeRepositoryTest()
        {
             // Create a mock for AutoMapper IMapper
            var myProfile = new EmployeeProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            var dbOptions = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString() // To create database every test
                );

            _context = new DataDbContext(dbOptions.Options);
            
        }
        [Fact]
        public async Task CreateEmployee()
        {
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            
            // Act
            var createEmployee = await employeeRepository.Create(employee); 
            Assert.NotEqual(0, createEmployee);

        } 
        [Fact]
        public async Task GetEmployeeById()
        {
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            
            // Act
            var createEmployee = await employeeRepository.Create(employee); 
            var result = await employeeRepository.GetById(createEmployee);
            Assert.NotNull(result);
            Assert.Equal(employee.FullName, result.FullName);

        }  

        [Fact]
        public async Task GetAllEmployees()
        {
            // Arrange
            var employees  = new List<EmployeeEntity>()
            {
                new() { Id=1,FullName = "Juan Dela Cruz",Birthdate = new DateTime(1996,03,29),Tin = "12345",TypeId = 1  },
                new() { Id=2,FullName = "John Cruz",Birthdate = new DateTime(1996,03,29),Tin = "12346",TypeId = 2  }
            };
            
            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            var employeeRepository = new EmployeeRepository(_context,_mapper);

            // Act
            var result = await employeeRepository.GetAll();
            
            // Asert
            Assert.NotNull(result);
            Assert.Equal(2,result.Count()); 
        } 

        [Fact]
        public async Task UpdateEmployee()
        {
            // Arrange
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

        
            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            var createEmployee = await employeeRepository.Create(employee); 
            var updatedEmployee = new Employee()
            {
                Id= createEmployee,
                FullName = "Juan Dela Cruz 2",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            // Act  
            await employeeRepository.Update(createEmployee,updatedEmployee);
            var updatedEmployeeResult = await employeeRepository.GetById(createEmployee);

            // Assert
            Assert.NotNull(updatedEmployeeResult);
            Assert.Equal(updatedEmployee.FullName, updatedEmployeeResult.FullName);

        }  

        [Fact]
        public async Task DeleteEmployee()
        {
            // Arrange
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

        
            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            var createEmployee = await employeeRepository.Create(employee); 

            // Act
            await employeeRepository.Delete(createEmployee);
            var result = await employeeRepository.GetById(createEmployee);

            // Asert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateExistingEmployeeFullName()
        {
            // Arrange
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            var createEmployee = await employeeRepository.Create(employee); 

            var employee1 =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "123456",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            // Act and Assert 
            _ = Assert.ThrowsAsync<DbUpdateException>(async () => await employeeRepository.Create(employee1));

        }

        [Fact]
        public async Task CreateExistingEmployeeTin()
        {
            // Arrange
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            var createEmployee = await employeeRepository.Create(employee); 

            var employee1 =  new Employee()
            {
                FullName = "Juan Dela Cruz 2",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            // Act and Assert 
            _ = Assert.ThrowsAsync<DbUpdateException>(async () => await employeeRepository.Create(employee1));

        }

        [Fact]
        public async Task UpdateEmployeeExistingFullName()
        {
            // Arrange
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

        
            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            var createEmployee = await employeeRepository.Create(employee); 
            var updatedEmployee = new Employee()
            {
                Id= createEmployee,
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "123456",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            // Act and Assert
            _ = Assert.ThrowsAsync<DbUpdateException>(async () =>  await employeeRepository.Update(createEmployee,updatedEmployee));

        }  

        [Fact]
        public async Task UpdateEmployeeExistingTin()
        {
            // Arrange
            var employee =  new Employee()
            {
                FullName = "Juan Dela Cruz",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

        
            var employeeRepository = new EmployeeRepository(_context,_mapper); 
            var createEmployee = await employeeRepository.Create(employee); 
            var updatedEmployee = new Employee()
            {
                Id= createEmployee,
                FullName = "Juan Dela Cruz 2",
                Birthdate ="1996-03-29",
                Tin = "12345",
                TypeId = Common.Enums.EmployeeType.Regular 
            };

            // Act and Assert
            _ = Assert.ThrowsAsync<DbUpdateException>(async () =>  await employeeRepository.Update(createEmployee,updatedEmployee));

        }  
    }
}