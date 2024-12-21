using Api.Models;

namespace Api.Providers
{
    public interface IEmployeeServiceProvider
    {
        Paycheck CalculatePaycheck(Employee employee);
    }
}
