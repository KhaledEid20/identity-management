using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace identityManagement.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AccountsController : ControllerBase
    {
        public IUnitofwork _unit {get; set;}
        public AccountsController(IUnitofwork unit)
        {
            _unit = unit;
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto user){
            if(ModelState.IsValid){
                return Ok(await _unit._authentication.Login(user));
            };
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(RegisterDto user){
            if(ModelState.IsValid){
                return Ok(await _unit._authentication.Register(user));
            };
            return BadRequest();
        }
    }
}