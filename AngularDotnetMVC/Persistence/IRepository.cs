using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence;

public interface IRepository<TEntity> where TEntity : class, new()
{
    Task<IQueryable<TEntity>> GetAllAsync();

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task DeleteAsync(object id);
}

