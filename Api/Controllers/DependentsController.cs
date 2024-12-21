using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public DependentsController(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _repository.GetDependentByIdAsync(id);
        if (dependent == null)
        {
            return NotFound(new ApiResponse<GetDependentDto> { Success = false, Error = "Dependent not found" });
        }

        var dependentDto = _mapper.Map<GetDependentDto>(dependent);
        return new ApiResponse<GetDependentDto> { Data = dependentDto, Success = true };
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _repository.GetAllDependentsAsync();
        var dependentDtos = _mapper.Map<List<GetDependentDto>>(dependents);
        return new ApiResponse<List<GetDependentDto>> { Data = dependentDtos, Success = true };
    }
}
