using MISA.Salary.Domain.Abstract;

namespace MISA.Salary.Domain.Repositories;
public interface IBaseRepository<TEntity, in TKey>
    where TEntity : IEntity<TKey>
    where TKey : notnull
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids);
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> UpdatePartialAsync(TEntity entity, IEnumerable<string> propertiesToUpdate);
    Task DeleteAsync(TKey id);
    Task<int> BulkDeleteAsync(IEnumerable<TKey> ids);
    Task<bool> ExistsAsync(TKey id);
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageSize, int pageIndex);
}
