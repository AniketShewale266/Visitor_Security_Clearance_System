using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using VisitorSecurityClearanceSystem.DTO;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : ControllerBase
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
        public SecurityController(IVisitorInterface visitorInterface)
        {
            _container = GetContainer();
            _visitorInterface = visitorInterface;
        }

        [HttpGet("pending-requests")]
        public IActionResult GetPendingVisitorRequests()
        {
            var pendingRequests = _visitorInterface.GetPendingVisitorRequests();
            return Ok(pendingRequests);
        }

        [HttpGet("all-requests")]
        public IActionResult GetAllVisitorRequests()
        {
            var allRequests = _visitorInterface.GetAllVisitorRequests();
            return Ok(allRequests);
        }

    }
}
