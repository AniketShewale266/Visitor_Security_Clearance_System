using VisitorSecurityClearanceSystem.DTO;
using VisitorSecurityClearanceSystem.Entities;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IUserInterface
    {
        Task<UserEntity> UserRegister(UserEntity user);
        
        Task<string> LoginUser(UserEntity user);
    }
}
