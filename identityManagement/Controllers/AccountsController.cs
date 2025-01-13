using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(RegisterDto user){
            if(ModelState.IsValid){
                return Ok(await _unit._authentication.Register(user));
            };
            return BadRequest();
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> refresh(TokenDto tokenDto){
            return Ok(await _unit._authentication.RefreshToken(tokenDto));
    }
        [HttpPost("confirmEmail")]
        public async Task<IActionResult> confirmEmail(string email , string code){
            var a = await _unit._email.sendEmail(email,code);
            if(a == "1"){
                return BadRequest("The input is invalid");
            }
            if(a == "2"){
                return BadRequest("The user does not exist");  
            }
            if (a == "3"){
                return Ok("The email has been confirmed");
            }
            return BadRequest("The email can't be confirmed");
        }
    }
}