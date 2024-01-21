using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VisitorSecurityClearanceSystem.DTO;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Services
{
    public class VisitorService : IVisitorInterface
    {
        public readonly Container _container;
        private Container GetContainer()
        {
            string URI = Environment.GetEnvironmentVariable("URI");
            string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
            string DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
            string ContainerName = Environment.GetEnvironmentVariable("ContainerNameL");
            CosmosClient cosmosclient = new CosmosClient(URI, PrimaryKey);
            //Connect with Our Database
            Database database = cosmosclient.GetDatabase(DatabaseName);
            //Connect with Our Container 
            Container container = database.GetContainer(ContainerName);

            return container;
        }
        public VisitorService()
        {
            _container = GetContainer();
        }

        public async Task<VisitorEntity> VisitorRegister(VisitorEntity visitor)
        {
            visitor.Id = Guid.NewGuid().ToString(); // 16 didit hex code
            visitor.VisitorId = visitor.Id;

            visitor.entryTime = DateTime.Now;
            visitor.exitTime = DateTime.Now;

            visitor.Status = "Pending";
            VisitorEntity resposne = await _container.CreateItemAsync(visitor);

            return resposne;
        }

        public async Task<VisitorEntity> VisitorById(string visitorId)
        {
            try
            {
                ItemResponse<VisitorEntity> res = await _container.ReadItemAsync<VisitorEntity>(visitorId, new PartitionKey(visitorId));

                return res.Resource;

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error while reading book with ID {visitorId}", ex);
            }
        }

        public List<VisitorDTO> GetPendingVisitorRequests()
        {
            var pendingVisitors = _container.GetItemLinqQueryable<VisitorEntity>()
                .Where(v => v.Status == "Pending")
                .ToFeedIterator();

            var result = new List<VisitorDTO>();

            while (pendingVisitors.HasMoreResults)
            {
                var response = pendingVisitors.ReadNextAsync().Result;

                foreach (var visitor in response)
                {
                    // Map VisitorEntity to VisitorDTO, you can create a method for mapping
                    var visitorDTO = new VisitorDTO
                    {
                        Name = visitor.Name,
                        Email = visitor.Email,
                        PhoneNumber = visitor.PhoneNumber,
                        Address = visitor.Address,
                        Purpose = visitor.Purpose,
                        VisitingTo = visitor.VisitingTo
                    };

                    result.Add(visitorDTO);
                }
            }

            return result;
        }

        public void ApproveVisitorRequest(string visitorId)
        {

            VisitorEntity visitor = _container.ReadItemAsync<VisitorEntity>(visitorId, new PartitionKey(visitorId)).Result;

            if (visitor != null && visitor.Status == "Pending")
            {
                // Update the status to "Approved"
                visitor.Status = "Approved";

                // Replace the existing document with the updated visitor entity
                _container.ReplaceItemAsync(visitor, visitorId).Wait();
            }
        }

        public void RejectVisitorRequest(string visitorId)
        {

            // Fetch the visitor with the specified userId
            VisitorEntity visitor = _container.ReadItemAsync<VisitorEntity>(visitorId, new PartitionKey(visitorId)).Result;

            if (visitor != null && visitor.Status == "Pending")
            {
                // Update the status to "Rejected"
                visitor.Status = "Rejected";

                // Replace the existing document with the updated visitor entity
                _container.ReplaceItemAsync(visitor, visitorId).Wait();
            }

        }

        public List<VisitorDTO> GetAllVisitorRequests()
        {
            var pendingVisitors = _container.GetItemLinqQueryable<VisitorEntity>()
                .Where(v => v.Status == "Pending" || v.Status == "Approved" || v.Status == "Rejected")
                .ToFeedIterator();

            var result = new List<VisitorDTO>();

            while (pendingVisitors.HasMoreResults)
            {
                var response = pendingVisitors.ReadNextAsync().Result;

                foreach (var visitor in response)
                {
                    // Map VisitorEntity to VisitorDTO, you can create a method for mapping
                    var visitorDTO = new VisitorDTO
                    {
                        Name = visitor.Name,
                        Email = visitor.Email,
                        PhoneNumber = visitor.PhoneNumber,
                        Address = visitor.Address,
                        Purpose = visitor.Purpose,
                        VisitingTo = visitor.VisitingTo
                    };

                    result.Add(visitorDTO);
                }
            }

            return result;
        }

    }
}
