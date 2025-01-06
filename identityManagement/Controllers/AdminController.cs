using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace identityManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public IUnitofwork _unit{get; set;}
        public AdminController(IUnitofwork unit)
        {
            _unit=unit;
        }
        [HttpPost("assignRoleToUser")]
        public async Task<ActionResult> roleToUser(string Email , string Role){
            if(ModelState.IsValid){
                return Ok(await _unit._admin.assignRoleToUser(Email , Role));
            }
            else{
                return BadRequest();
            }
        }
        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRole(string Role){
            if(ModelState.IsValid){
                return Ok(await _unit._admin.AddRole(Role));
            }
            else{
                return BadRequest();
            }
        }
        [HttpPost("AddClaimToUser")]
        public async Task<ActionResult> AddClaimToUser(string Email, string ClaimName , string ClaimValue){
            if(ModelState.IsValid){
                return Ok(await _unit._claims.addClaimToUser(Email , ClaimName , ClaimValue));
            }
            else{
                return BadRequest();
            }
        }
        [HttpPost("addClaimTORole")]
        public async Task<ActionResult> ClaimToRole(string Rolename, string claimName, string ClaimValue){
            if(ModelState.IsValid){
                return Ok(await _unit._claims.addClaimToRole(Rolename , claimName , ClaimValue));
            }
            else{
                return BadRequest();
            }
        }
    }
}
