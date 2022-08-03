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
        private readonly FirebaseRepository firebaseRepository;

        public UsersController(FirebaseRepository firebaseRepository)
        {
            this.firebaseRepository = firebaseRepository;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetAllUsers()
        {
            return Ok("dla admina");
        }

        [HttpPost("firebasetest")]
        public async Task TestFirebase([FromBody] EnergyProduction energyProduction, CancellationToken cancellationToken)
        {
            await firebaseRepository.AddEnergyProduction(energyProduction, cancellationToken);
        }

        [HttpPost("multiaddenergy")]
        public async Task AddMultiple([FromBody] AddEnergyProductionsRequest addEnergyProductionsRequest, CancellationToken cancellationToken)
        {
            await firebaseRepository.AddEnergyProductions(addEnergyProductionsRequest, cancellationToken);    
        }
    }
}
