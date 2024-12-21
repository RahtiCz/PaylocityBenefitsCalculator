using AutoMapper;
using Api.Dtos.Employee;
using Api.Models;
using Api.Dtos.Dependent;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, GetEmployeeDto>();
        CreateMap<Dependent, GetDependentDto>();
    }
}