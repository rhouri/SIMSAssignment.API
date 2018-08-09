
using System.Threading.Tasks;
using SIMSAssignment.API.Models;
using Microsoft.AspNetCore.Mvc;
using SIMSAssignment.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace SIMSAssignment.API.Controllers
    {
    [Route("api/auth")]
    public class AuthController :Controller
        {
        private readonly UsersService usersService;

        public AuthController ( UsersService usersService )
            {
            this.usersService = usersService;
            }
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Token ( [FromBody] LoginModel model )
            {
            if (!ModelState.IsValid)
                {
                return BadRequest(ModelState);
                }

            var result = await usersService.Login(model);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result.Result);
            }
        }
    }
