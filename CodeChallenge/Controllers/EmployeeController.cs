using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("{id}/reporting-structure", Name = "getReportingStructure")]
        public async Task<IActionResult> GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received employee get reporting structure request for '{id}'");

            // Check if emplyee exist
            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            var employeeReportingStructure = _employeeService.GetReportingStructureById(employee, id);

            if (employeeReportingStructure == null)
                return NotFound();

            return Ok(employeeReportingStructure);
        }

        /// <summary>
        /// Create compensation 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="compensation"></param>
        /// <returns></returns>
        [HttpPost("{id}/compensation", Name = "createCompensation")]
        public IActionResult CreateCompensation(string Id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for employee '{Id}' -- Salary: '{compensation.Salary}' -- EffectiveDate '{compensation.EffectiveDate}'");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if employee exist before client can set compensation
            var employee = _employeeService.GetById(compensation.Employee.EmployeeId);

            if (employee == null)
            {
                return NotFound();
            }

            compensation.EmployeeId = Id;
            compensation.Employee = null;

            // Create a new compensation to our db context
            _employeeService.CreateCompensation(compensation);

            return Ok(compensation);
        }

        /// <summary>
        /// Gets latest record of compensation for employee
        /// </summary>
        /// <param name="id"></param>
        /// <param name="getLatestCompensation"></param>
        /// <returns></returns>
        [HttpGet("{id}/compensation", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id, bool getLatestCompensation)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            var compensation = _employeeService.GetCompensationByEmployeeId(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
