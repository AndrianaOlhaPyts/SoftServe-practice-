using Cinema.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.Repository.Interface
{
    public interface IHallRepository : IGenericRepository<Hall>
    {
        Task<Hall> GetHallByIdAsync(Guid hallId);
    }
}
