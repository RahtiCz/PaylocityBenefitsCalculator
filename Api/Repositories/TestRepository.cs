﻿using Api.Models;
using System.Collections.Concurrent;

namespace Api.Repositories
{
    public class TestRepository : IEmployeeRepository
    {
        private readonly ConcurrentDictionary<int, Employee> _employees = new();

        public TestRepository()
        {
            var initialEmployees = GetInitialEmployees();

            foreach (var employee in initialEmployees)
            {
                _employees[employee.Id] = employee;
            }
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult<IEnumerable<Employee>>(_employees.Values);
        }

        public Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            _employees.TryGetValue(id, out var employee);
            return Task.FromResult(employee);
        }

        public Task<IEnumerable<Dependent>> GetAllDependentsAsync()
        {
            var dependents = _employees.Values.SelectMany(e => e.Dependents).ToList();
            return Task.FromResult<IEnumerable<Dependent>>(dependents);
        }

        public Task<Dependent?> GetDependentByIdAsync(int id)
        {
            var dependent = _employees.Values.SelectMany(e => e.Dependents).FirstOrDefault(d => d.Id == id);
            return Task.FromResult(dependent);
        }

        private static List<Employee> GetInitialEmployees()
        {
            return new List<Employee>
            {
                new() {
                    Id = 1,
                    FirstName = "LeBron",
                    LastName = "James",
                    Salary = 75420.99m,
                    DateOfBirth = new DateTime(1984, 12, 30)
                },
                new() {
                    Id = 2,
                    FirstName = "Ja",
                    LastName = "Morant",
                    Salary = 92365.22m,
                    DateOfBirth = new DateTime(1999, 8, 10),
                    Dependents = new List<Dependent>
                    {
                        new() {
                            Id = 1,
                            FirstName = "Spouse",
                            LastName = "Morant",
                            Relationship = Relationship.Spouse,
                            DateOfBirth = new DateTime(1998, 3, 3)
                        },
                        new() {
                            Id = 2,
                            FirstName = "Child1",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2020, 6, 23)
                        },
                        new() {
                            Id = 3,
                            FirstName = "Child2",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2021, 5, 18)
                        }
                    }
                },
                new() {
                    Id = 3,
                    FirstName = "Michael",
                    LastName = "Jordan",
                    Salary = 143211.12m,
                    DateOfBirth = new DateTime(1963, 2, 17),
                    Dependents = new List<Dependent>
                    {
                        new() {
                            Id = 4,
                            FirstName = "DP",
                            LastName = "Jordan",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1974, 1, 2)
                        }
                    }
                },
                new() {
                    Id = 4,
                    FirstName = "Petr",
                    LastName = "Rich",
                    Salary = 91000m,
                    DateOfBirth = new DateTime(1970, 1, 1),
                    Dependents = new List<Dependent>
                    {
                        new() {
                            Id = 5,
                            FirstName = "DP",
                            LastName = "Rich",
                            Relationship = Relationship.Spouse,
                            DateOfBirth = new DateTime(1974, 1,1)
                        },
                        new() {
                            Id = 6,
                            FirstName = "DP",
                            LastName = "Poor",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1990, 1, 1)
                        }
                    }
                }
            };
        }
    }
}
