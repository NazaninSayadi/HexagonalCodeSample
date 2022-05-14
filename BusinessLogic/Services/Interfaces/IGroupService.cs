using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDTO>> GetAll();
        Task<GroupDTO> Get(Guid Id);
        Task<Guid> Add(string name, decimal capacity);
        Task Update(Guid id, string name, decimal capacity);
        Task Remove(Guid Id);

    }
}
