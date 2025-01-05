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
        [HttpGet("assignRoleToUser")]
        public async Task<ActionResult> roleToUser(string Email , string Role){
            return Ok(await _unit._admin.assignRoleToUser(Email , Role));
        }
        [HttpGet("AddRole")]
        public async Task<ActionResult> AddRole(string Role){
            return Ok(await _unit._admin.AddRole(Role));
        }
        [HttpGet("AddClaimToUser")]
        public async Task<ActionResult> AddClaimToUser(string Email, string ClaimName , string ClaimValue){
            return Ok(await _unit._claims.addClaimToUser(Email , ClaimName , ClaimValue));
        }
        [HttpGet("addClaimTORole")]
        public async Task<ActionResult> ClaimToRole(string Rolename, string claimName, string ClaimValue){
            return Ok(await _unit._claims.addClaimToRole(Rolename , claimName , ClaimValue));
        }
    }
}
