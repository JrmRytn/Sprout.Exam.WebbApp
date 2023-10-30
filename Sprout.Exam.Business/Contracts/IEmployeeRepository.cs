using System.Collections.Generic;
using System.Threading.Tasks; 
using Sprout.Exam.Business.Models;

namespace Sprout.Exam.Business.Contracts
{
    public interface IEmployeeRepository
    {
        Task<int> Create(Employee employee);
        Task<Employee> GetById(int id);
        Task<IEnumerable<Employee>>  GetAll();
        Task Update(int id,Employee employee); 
        Task Delete(int id);
    }
}