using Api.Models;

namespace Api.Providers
{
    public interface IEmployeeServiceProvider
    {
        Paycheck CalculatePaycheck(Employee employee);
        bool ValidateDependents(ICollection<Dependent> dependents, out string errorMessage);
    }
}
