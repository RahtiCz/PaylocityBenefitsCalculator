using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public EmployeesController(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound(new ApiResponse<GetEmployeeDto> { Success = false, Error = "Employee not found" });
        }

        var employeeDto = _mapper.Map<GetEmployeeDto>(employee);
        return new ApiResponse<GetEmployeeDto> { Data = employeeDto, Success = true };
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _repository.GetAllEmployeesAsync();
        var employeeDtos = _mapper.Map<List<GetEmployeeDto>>(employees);
        return new ApiResponse<List<GetEmployeeDto>> { Data = employeeDtos, Success = true };
    }
}
