using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IFineRepository
    {
        Task<List<Fine>> GetAll();
        Task<List<Fine>> GetByUserId(string userId);
        Task<Fine?> GetById(Guid Id);
        Task AddAsync(Fine fine);
        Task UpdateAsync(Fine fine);
    }
}
