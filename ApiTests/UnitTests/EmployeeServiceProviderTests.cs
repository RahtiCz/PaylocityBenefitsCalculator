using Api.Models;
using Api.Providers;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiTests.UnitTests
{
    public class EmployeeServiceProviderTests
    {
        private readonly EmployeeServiceProvider _serviceProvider;

        public EmployeeServiceProviderTests()
        {
            _serviceProvider = new EmployeeServiceProvider();
        }

        [Theory]
        [MemberData(nameof(GetEmployeeTestData))]
        public void CalculatePaycheck_CorrectlyCalculatesPaycheck(
            Employee employee,
            decimal expectedEarnings,
            decimal expectedDeductions,
            decimal expectedNetPay)
        {
            // Act
            var paycheck = _serviceProvider.CalculatePaycheck(employee);
            
            // Assert
            Assert.Equal(expectedEarnings, paycheck.Earnings);
            Assert.Equal(expectedDeductions, paycheck.Deductions);
            Assert.Equal(expectedNetPay, paycheck.NetPay);
        }

        public static IEnumerable<object[]> GetEmployeeTestData()
        {
            const decimal BaseDeduction = 1000m * 12;
            const decimal DependentCost = 600m * 12;
            const decimal Over50Cost = 200m * 12;

            yield return new object[]
            {
            // Test case: Employee without dependents under high salary
            new Employee
            {
                Id = 1,
                FirstName = "Petr",
                LastName = "Poor",
                Salary = 65000,
                DateOfBirth = new DateTime(1970, 1, 1),
                Dependents = new List<Dependent>()
            },
            65000m / 26, 
            BaseDeduction / 26, 
            (65000m / 26) - (BaseDeduction / 26)
            };

            yield return new object[]
            {
            // Test case: Employee without dependents with high salary
            new Employee
            {
                Id = 2,
                FirstName = "Petr",
                LastName = "Rich",
                Salary = 91000,
                DateOfBirth = new DateTime(1970, 1, 1),
                Dependents = new List<Dependent>()
            },
            91000m / 26,
            (BaseDeduction + (91000m * 0.02m)) / 26,
            (91000m / 26) - ((BaseDeduction + (91000m * 0.02m)) / 26)
            };

            yield return new object[]
            {
            // Test case: Employee with dependents under 50
            new Employee
            {
                Id = 3,
                FirstName = "Petr",
                LastName = "Poor",
                Salary = 65000,
                DateOfBirth = new DateTime(1970, 1, 1),
                Dependents = new List<Dependent>
                {
                    new() { Id = 1, FirstName = "Girl", LastName = "Poor", Relationship = Relationship.Child, DateOfBirth = new DateTime(2000, 1, 1) },
                    new() { Id = 2, FirstName = "Boy", LastName = "Poor", Relationship = Relationship.Child, DateOfBirth = new DateTime(2012, 1, 1) }
                }
            },
            65000m / 26,
            (BaseDeduction + (DependentCost * 2)) / 26,
            (65000m / 26) - ((BaseDeduction + (DependentCost * 2)) / 26)
            };

            yield return new object[]
            {
            // Test case: Employee with dependent over 50
            new Employee
            {
                Id = 4,
                FirstName = "Petr",
                LastName = "Poor",
                Salary = 65000,
                DateOfBirth = new DateTime(2000, 1, 1),
                Dependents = new List<Dependent>
                {
                    new() { Id = 1, FirstName = "Girl", LastName = "Poor", Relationship = Relationship.Child, DateOfBirth = new DateTime(2023, 1, 1) },
                    new() { Id = 2, FirstName = "Wife", LastName = "Poor", Relationship = Relationship.Spouse, DateOfBirth = new DateTime(1973, 1, 1) }
                }
            },
            65000m / 26,
            (BaseDeduction + DependentCost + (DependentCost + Over50Cost)) / 26,
            (65000m / 26) - ((BaseDeduction + DependentCost + (DependentCost + Over50Cost)) / 26)
            };

            yield return new object[]
            {
            // Test case: Employee with high salary and dependent over 50
            new Employee
            {
                Id = 5,
                FirstName = "Petr",
                LastName = "Rich",
                Salary = 91000,
                DateOfBirth = new DateTime(2000, 1, 1),
                Dependents = new List<Dependent>
                {
                    new() { Id = 1, FirstName = "Girl", LastName = "Rich", Relationship = Relationship.Child, DateOfBirth = new DateTime(2023, 1, 1) },
                    new() { Id = 2, FirstName = "Wife", LastName = "Rich", Relationship = Relationship.Spouse, DateOfBirth = new DateTime(1973, 1, 1) }
                }
            },
            91000m / 26,
            (BaseDeduction + (91000m * 0.02m) + DependentCost + (DependentCost + Over50Cost)) / 26,
            (91000m / 26) - ((BaseDeduction + (91000m * 0.02m) + DependentCost + (DependentCost + Over50Cost)) / 26)
            };
        }
    }

}
