using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Providers;
using Api.Repositories;
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
    private readonly IEmployeeServiceProvider _employeeServiceProvider;

    public EmployeesController(IEmployeeRepository repository, IMapper mapper, IEmployeeServiceProvider employeeServiceProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _employeeServiceProvider = employeeServiceProvider;
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

    [SwaggerOperation(Summary = "Calculate paycheck for employee by id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> CalculatePaycheck(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound(new ApiResponse<decimal> { Success = false, Error = "Employee not found" });
        }

        //This should be in create/update employee endpoint. But just for demostration of the validation. I should not return 400, but just for simplicity.
        if (!_employeeServiceProvider.ValidateDependents(employee.Dependents, out var errorMessage))
        {
            return BadRequest(new ApiResponse<GetPaycheckDto> { Success = false, Error = errorMessage });
        }

        var paycheck = _employeeServiceProvider.CalculatePaycheck(employee);
        var paycheckDto = _mapper.Map<GetPaycheckDto>(paycheck);
        return new ApiResponse<GetPaycheckDto> { Data = paycheckDto, Success = true };
    }
}
