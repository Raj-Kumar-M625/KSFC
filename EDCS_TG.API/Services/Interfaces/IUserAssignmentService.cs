using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IUserAssignmentService
    {
        Task<IEnumerable<joinModel>> GetAllAssignedUsers();

        Task<UserAssignment> GetAssignedUserById(int id);

        Task<UserAssignment> AssignUser(UserAssignment userAssignment);

        Task<UserAssignment> RemoveUser(Guid id);

        Task<IEnumerable<UserAssignment>> getAssignedUserByDistrict(string district);

        Task<IEnumerable<UserAssignment>> getAssignedUserByTaluk(string taluk);

        Task<IEnumerable<UserAssignment>> getAssignedUserByHobli(string hobli);
    }
}
