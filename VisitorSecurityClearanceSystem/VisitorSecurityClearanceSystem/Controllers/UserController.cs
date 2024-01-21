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
    public class UserController : ControllerBase
    {
        public readonly Container _container;
        public IUserInterface _userInterface;

        private Container GetContainer()
        {
            string URI = Environment.GetEnvironmentVariable("URI");
            string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
            string DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
            string ContainerName = Environment.GetEnvironmentVariable("ContainerName");
            CosmosClient cosmosclient = new CosmosClient(URI, PrimaryKey);
            //Connect with Our Database
            Database databse = cosmosclient.GetDatabase(DatabaseName);
            //Connect with Our Container 
            Container container = databse.GetContainer(ContainerName);

            return container;
        }

        public UserController(IUserInterface managerInterface)
        {
            _container = GetContainer();
            _userInterface = managerInterface;
        }
        [HttpPost("register")]
        public async Task<IActionResult> UserRegister(UserDTO userDTO)
        {
            UserEntity userEntity = new UserEntity();
            // Convert user model to user entity
            userEntity.Username = userDTO.Username;
            userEntity.Password = userDTO.Password;
            userEntity.Role = userDTO.Role;
     

            // Call service function
            var responce = await _userInterface.UserRegister(userEntity);
            // Return model to UI
            userDTO.Username = responce.Username;
            userDTO.Password = responce.Password;
            userDTO.Role = responce.Role;

            return Ok(userDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserEntity userEntity)
        {
            var result = await _userInterface.LoginUser(userEntity);

            if (result != null)
            {
                return Ok(new {
                    Message = "Login successful",
                    UId = result 
                });
            }

            return Unauthorized(new { Message = "Invalid credentials." });
        }
    }
}
