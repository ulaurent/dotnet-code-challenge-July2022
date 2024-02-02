using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        // Create compensation
        // Add compensation to DBContext
        // SaveAsync.
        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation != null)
            {
                _employeeRepository.Add(compensation);
                _employeeRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public List<Compensation> GetCompensationByEmployeeId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetCompensationListByEmployeeId(id);
            }

            return null;
        }

        public ReportingStructure GetReportingStructureById(Employee parentEmployee, string id)
        {
            // Breadth First Search
            ReportingStructure reportingStructure = new ReportingStructure();
            reportingStructure.Employee = parentEmployee;

            if (!String.IsNullOrEmpty(id))
            {
                Queue<Employee> employeeQueue = new Queue<Employee>();

                employeeQueue.Enqueue(parentEmployee);

                Dictionary<string, Employee> employeeDictionary = new Dictionary<string, Employee>();

                Employee currentEmployee = null;

                while(employeeQueue.Count != 0)
                {
                    // remove & gets object from queue
                    currentEmployee = employeeQueue.Dequeue();

                    // if node has children
                    if(currentEmployee.DirectReports != null)
                    {
                        foreach (Employee employee in currentEmployee.DirectReports)
                        {
                            if (!employeeDictionary.ContainsKey(employee.EmployeeId))
                            {
                                // Add the employee to the queue
                                employeeQueue.Enqueue(employee);
                                employeeDictionary.Add(employee.EmployeeId, employee);
                            }
                        }
                    }
                }

                reportingStructure.NumberOfReports = employeeDictionary.Count;
            }

            return reportingStructure;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if (originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
