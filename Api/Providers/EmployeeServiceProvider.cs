using Api.Models;

namespace Api.Providers
{
    public class EmployeeServiceProvider : IEmployeeServiceProvider
    {
        private const decimal BaseMonthlyCost = 1000;
        private const decimal DependentAdditionalCost = 600;
        private const decimal HighSalaryAdditionalRate = 0.02m;
        private const decimal DependentOver50AdditionalCost = 200;
        private const int PaychecksPerYear = 26;

        public Paycheck CalculatePaycheck(Employee employee)
        {
            var paycheckEarnings = employee.Salary / PaychecksPerYear;
            var paycheckDeductions = CalculateYearlyDeductions(employee)/ PaychecksPerYear;

            return new Paycheck
            {
                Earnings = paycheckEarnings,
                Deductions = paycheckDeductions,
                NetPay = paycheckEarnings - paycheckDeductions
            };
        }
        private decimal CalculateYearlyDeductions(Employee employee)
        {
            //Base cost for employee
            decimal baseCost = BaseMonthlyCost * 12;

            //Base cost for dependents
            decimal dependentCost = employee.Dependents.Sum(d => DependentAdditionalCost + (IsDependentOver50(d) ? DependentOver50AdditionalCost : 0)) * 12;

            //Additional cost for high salary
            decimal additionalCost = employee.Salary > 80000 ? (employee.Salary * HighSalaryAdditionalRate)  : 0;

            return baseCost + dependentCost + additionalCost;
        }

        private bool IsDependentOver50(Dependent dependent)
        {
            return dependent.DateOfBirth.AddYears(50) <= DateTime.Now;
        }
    }
}
