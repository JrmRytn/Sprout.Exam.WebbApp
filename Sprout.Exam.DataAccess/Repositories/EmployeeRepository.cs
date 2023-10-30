using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.Contracts;
using Sprout.Exam.Business.Exceptions;
using Sprout.Exam.Business.Models;
using Sprout.Exam.DataAccess.Data;
using Sprout.Exam.DataAccess.Entities;

namespace Sprout.Exam.DataAccess.Repositories
{ 
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataDbContext _context;
        private readonly IMapper _mapper; 

        public EmployeeRepository(DataDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        } 
        public async Task<int> Create(Employee employee)
        {
            var entity = _mapper.Map<EmployeeEntity>(employee);
            _context.Employees.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Employees
                .ProjectTo<Employee>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _context.Employees
                .Where(w => w.Id == id)
                .ProjectTo<Employee>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task Update(int id, Employee employee)
        {
            var employeEntity = await _context.Employees.FindAsync(id) ?? throw new NotFoundException("Employee not found.");
            var updatedEntity = _mapper.Map<EmployeeEntity>(employee);
            _context.Entry(employeEntity).CurrentValues.SetValues(updatedEntity);
            _context.Entry(employeEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public async Task Delete(int id)
        {
            var employeEntity = await _context.Employees.FindAsync(id) ?? throw new NotFoundException("Employee not found.");
            _context.Employees.Remove(employeEntity);
            await _context.SaveChangesAsync();
        }


    }


}