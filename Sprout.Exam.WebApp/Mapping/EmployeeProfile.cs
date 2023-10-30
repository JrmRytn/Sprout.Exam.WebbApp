using System;
using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.Models;
using Sprout.Exam.DataAccess.Entities;


namespace Sprout.Exam.WebApp.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto,Employee>()
                .ConstructUsing((s) => new Employee(0,s.FullName,s.Birthdate,s.Tin,s.TypeId));

            CreateMap<EditEmployeeDto,Employee>()
                .ConstructUsing((s) => new Employee(s.Id,s.FullName,s.Birthdate,s.Tin,s.TypeId));

            CreateMap<Employee,EmployeeDto>()
                .ForMember(d => d.TypeId , o => o.MapFrom(s => (short)s.TypeId));

            CreateMap<Employee, EmployeeEntity>()
                .ForMember(d => d.TypeId, o => o.MapFrom(s => (short)s.TypeId))
                .ForMember(d => d.Birthdate, o => o.MapFrom(s => Convert.ToDateTime(s.Birthdate)));
            
            CreateMap<EmployeeEntity, Employee>()
                .ConstructUsing((s) => new Employee(s.Id,s.FullName,s.Birthdate.ToString("yyyy-MM-dd"),s.Tin,s.TypeId));

                            

        }
    }
}