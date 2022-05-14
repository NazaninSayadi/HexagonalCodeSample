using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> Add(T entity);
        Task Update();
        Task Delete(T entity);
    }
}
