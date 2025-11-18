using MISA.Salary.Domain.Abstract;

namespace MISA.Salary.Domain.Repositories;
public interface IBaseRepository<TEntity, in TKey>
    where TEntity : IEntity<TKey>
    where TKey : notnull
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);
    Task<bool> ExistsAsync(TKey id);
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageSize, int pageIndex);
}
