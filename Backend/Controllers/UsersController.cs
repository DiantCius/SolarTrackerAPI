using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {


        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetAllUsers()
        {
            return Ok("dla admina");
        }

        [HttpGet("test")]
        public ActionResult GetAllTest()
        {
            return Ok("test");
        }

    }
}
