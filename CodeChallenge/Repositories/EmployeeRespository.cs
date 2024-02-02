using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        /// <summary>
        /// Add a new compensation to DB context.
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Employee GetById(string id)
        {
            // Direct Reports return null 
            // Applied ToList() to employees object to materialize as list 
            return _employeeContext.Employees.ToList().SingleOrDefault(e => e.EmployeeId == id);
        }

        /// <summary>
        /// Get compensation based on employee id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Compensation GetCompensationByEmployeeId(string id)
        {
            return _employeeContext.Compensations.Include(c => c.Employee).SingleOrDefault(c => c.Employee.EmployeeId == id);
        }


        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public List<Compensation> GetCompensationListByEmployeeId(string id)
        {
            return _employeeContext.Compensations.Where(c => c.Employee.EmployeeId == id).ToList();
        }
    }
}
