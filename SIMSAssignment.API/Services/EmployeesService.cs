using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SIMSAssignment.API.Domain;
using SIMSAssignment.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SIMSAssignment.API.Services
    {
    public class EmployeesService
        {
        private readonly IConfiguration configuration;
        private readonly SIMSDbContext dbContext;

        public EmployeesService ( IConfiguration configuration, SIMSDbContext db )
            {
            this.configuration = configuration;
            this.dbContext = db;
            }

        public async Task<Employee> Find ( ClaimsPrincipal user, int id )
            {
            var Emp= await dbContext.Employees.FindAsync(id);
            if (!user.Claims.Any(c => c.Type == "ViewSSN" && c.Value == "true"))
                Emp.SSN = "*********";
            if (!user.Claims.Any(c => c.Type == "ViewDL" && c.Value == "true"))
                Emp.DriverLicense = "*********";
            return Emp;
            }

        public async Task<List<Employee>> List ( ClaimsPrincipal user )
            {
            return await dbContext.Employees.Select(e=> new Employee // restrict data for list would use mapper in prod app
                {
                Department=e.Department,
                Email=e.Email,
                Id=e.Id,
                LastName=e.LastName,
                FirstName=e.FirstName,
                MiddleName=e.MiddleName,
                })
                .ToListAsync();
            }

        }
    }
