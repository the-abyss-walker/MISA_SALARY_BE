using System.Data;

namespace MISA.Salary.Domain.Repositories;
public interface IUnitOfWork : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }
    Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted);
    Task CommitAsync();
    Task RollbackAsync();
}
