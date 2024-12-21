using AutoMapper;
using Api.Dtos.Employee;
using Api.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, GetEmployeeDto>();
    }
}