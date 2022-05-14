using Domain.Entities;
using Domain.Repositories.Base;

namespace Domain.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Group> GetById(Guid groupId);
        Task<IReadOnlyList<Group>> GetAll();
    }
}
