using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using VisitorSecurityClearanceSystem.DTO;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        public readonly Container _container;
        public IVisitorInterface _visitorInterface;

        private Container GetContainer()
        {
            string URI = Environment.GetEnvironmentVariable("URI");
            string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
            string DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
            string ContainerName = Environment.GetEnvironmentVariable("ContainerNameL");
            CosmosClient cosmosclient = new CosmosClient(URI, PrimaryKey);
            //Connect with Our Database
            Database databse = cosmosclient.GetDatabase(DatabaseName);
            //Connect with Our Container 
            Container container = databse.GetContainer(ContainerName);

            return container;
        }

        public VisitorController(IVisitorInterface visitorInterface)
        {
            _container = GetContainer();
            _visitorInterface = visitorInterface;
        }

        [HttpPost("register")]
        public async Task<IActionResult> VisitorRegister(VisitorDTO visitorDTO)
        {
            VisitorEntity visitorEntity = new VisitorEntity();
            // Convert user model to user entity
            visitorEntity.Name = visitorDTO.Name;
            visitorEntity.PhoneNumber = visitorDTO.PhoneNumber;
            visitorEntity.Address = visitorDTO.Address;
            visitorEntity.Email = visitorDTO.Email;
            visitorEntity.Purpose = visitorDTO.Purpose;
            visitorEntity.VisitingTo = visitorDTO.VisitingTo;
     
      
            // Call service function
            var responce = await _visitorInterface.VisitorRegister(visitorEntity);
            // Return model to UI
            visitorDTO.Name = responce.Name;
            visitorDTO.PhoneNumber = responce.PhoneNumber;
            visitorDTO.Address = responce.Address;
            visitorDTO.Email = responce.Email;
            visitorDTO.Purpose = responce.Purpose;

            return Ok(visitorDTO);
        }


        [HttpGet("{visitorId}")]
        public async Task<IActionResult> VisitorById(string visitorId)
        {
            try
            {
                var visitor = await _visitorInterface.VisitorById(visitorId);

                if (visitor == null)
                {
                    return NotFound();
                }

                return Ok(visitor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
