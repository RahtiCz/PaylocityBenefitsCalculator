using Api.Models;

namespace Api.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);

        Task<IEnumerable<Dependent>> GetAllDependentsAsync();
        Task<Dependent?> GetDependentByIdAsync(int id);
    }
}
