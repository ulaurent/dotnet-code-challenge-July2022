using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Compensation Add(Compensation compensation);
        Compensation GetCompensationListByEmployeeId(String id);
        Task SaveAsync();
    }
}