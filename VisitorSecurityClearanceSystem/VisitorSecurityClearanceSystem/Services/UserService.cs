using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Services
{
    public class UserService: IUserInterface
    {
        public readonly Container _container;
        private Container GetContainer()
        {
            string URI = Environment.GetEnvironmentVariable("URI");
            string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
            string DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
            string ContainerName = Environment.GetEnvironmentVariable("ContainerName");
            CosmosClient cosmosclient = new CosmosClient(URI, PrimaryKey);
            //Connect with Our Database
            Database database = cosmosclient.GetDatabase(DatabaseName);
            //Connect with Our Container 
            Container container = database.GetContainer(ContainerName);

            return container;
        }
        public UserService()
        {
            _container = GetContainer();
        }

        public async Task<UserEntity> UserRegister(UserEntity user)
        {
            user.Id = Guid.NewGuid().ToString(); // 16 didit hex code
            user.UId = user.Id; // taskEntity.Id; 

            user.DocumentType = "User Information with its Role";

            user.CreatedOn = DateTime.Now;
            user.CreatedByName = "Aniket";
            user.CreatedBy = "Aniket UID";

            user.UpdatedOn = DateTime.Now;
            user.UpdatedByName = "Aniket";
            user.UpdatedBy = "Aniket UID";

            user.Version = 1;
            user.Active = true;
            user.Archieved = false;  // Not Accesible to System

            UserEntity resposne = await _container.CreateItemAsync(user);

            return resposne;
        }

        public async Task<string> LoginUser(UserEntity user)
        {
            var query = _container.GetItemLinqQueryable<UserEntity>()
                .Where(u => u.Username == user.Username && u.Password == user.Password && u.Role == user.Role)
                .Take(1)
                .ToFeedIterator();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                var matchedUser = response.FirstOrDefault();

                if (matchedUser != null)
                {
                    return matchedUser.UId;
                }
            }

            return null;
        }

    }
}
