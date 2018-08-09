using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMSAssignment.API.Domain;
using SIMSAssignment.API.Services;

namespace SIMSAssignment.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController :ControllerBase
        {

        private readonly EmployeesService employeesService;

        public EmployeesController ( EmployeesService srv )
            {
            this.employeesService = srv;
            }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get (int id)
            {
            var employee = await employeesService.Find(User, id);
            return Ok(employee);
            }

        [Authorize]
        [HttpGet("List")]
        public async Task<IActionResult> GetEmployeesList ()
            {
            var employees = await employeesService.List(User);
            return Ok(employees);
            }
        }
    }