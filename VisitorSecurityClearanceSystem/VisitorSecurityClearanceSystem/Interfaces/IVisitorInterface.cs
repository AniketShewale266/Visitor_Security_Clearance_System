using VisitorSecurityClearanceSystem.DTO;
using VisitorSecurityClearanceSystem.Entities;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IVisitorInterface
    {
        Task<VisitorEntity> VisitorRegister(VisitorEntity visitor);
        Task<VisitorEntity> VisitorById(string visitorId);

        List<VisitorDTO> GetPendingVisitorRequests();
        void ApproveVisitorRequest(string visitorId);
        void RejectVisitorRequest(string visitorId);

        List<VisitorDTO> GetAllVisitorRequests();


    }
}
