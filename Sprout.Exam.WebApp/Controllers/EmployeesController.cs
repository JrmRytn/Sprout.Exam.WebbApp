using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.Contracts;
using Sprout.Exam.Business.Exceptions;
using Sprout.Exam.Business.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper; 

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper; 
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeRepository.GetAll();
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeRepository.GetById(id);
            var employee = _mapper.Map<EmployeeDto>(result);
            return Ok(employee);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try
            {
                var employee = _mapper.Map<Employee>(input);
                await _employeeRepository.Update(employee.Id, employee);
                
                return Ok(employee);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                {  
                    // Extract the conflicting value from the full message
                    string conflictingIndex = sqlException.Message.Split('\'')[3];

                       // Extract the column name from the constraint name
                    string[] segments = conflictingIndex.Split('_');
                    string columnName = segments.Length > 1 ? segments[^1] : "UnknownColumn";

                    ModelState.AddModelError(columnName,$"{columnName} value already exist."); 
                    return ValidationProblem();
                }
                else
                {
                    throw;
                }
            } 
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        { 
            try
            {
                var newEntity = _mapper.Map<Employee>(input);
                var id = await _employeeRepository.Create(newEntity);
                return Created($"/api/employees/{id}", id);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                {  
                    // Extract the conflicting index from the message
                    string conflictingIndex = sqlException.Message.Split('\'')[3];

                    // Extract the column name from the index name
                    string[] segments = conflictingIndex.Split('_');
                    string columnName = segments.Length > 1 ? segments[^1] : "UnknownColumn";

                    ModelState.AddModelError(columnName,$"{columnName} value already exist."); 
                    return ValidationProblem();
                }
                else
                {
                    throw;
                }
            } 
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
        
            try
            {
                await _employeeRepository.Delete(id);
                return Ok(id);
            }catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        } 
        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>

        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id, [FromBody] CalculateDto calculateDto)
        { 
            try{
                
                var result = await _employeeRepository.GetById(id); 
                var calculatedSalary = result.CalculateSalary(calculateDto.WorkedDays ?? 0,calculateDto.AbsentDays ?? 0,calculateDto.WorkedHours ?? 0);
                return Ok(calculatedSalary);

            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
