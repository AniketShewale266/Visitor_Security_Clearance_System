using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using VisitorSecurityClearanceSystem.DTO;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfficeController : ControllerBase
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

        public OfficeController(IVisitorInterface visitorInterface)
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

        [HttpPost("approve-request/{visitorId}")]
        public IActionResult ApproveVisitorRequest(string visitorId)
        {
            _visitorInterface.ApproveVisitorRequest(visitorId);
            return Ok($"Visitor request with ID {visitorId} approved");
        }

        [HttpPost("reject-request/{visitorId}")]
        public IActionResult RejectVisitorRequest(string visitorId)
        {
            _visitorInterface.RejectVisitorRequest(visitorId);
            return Ok($"Visitor request with ID {visitorId} rejected");
        }
    }
}
