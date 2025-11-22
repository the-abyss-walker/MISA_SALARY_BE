using MISA.Salary.Domain.Repositories;
using System.Data;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class BaseUnitOfWork : IUnitOfWork
{
    private readonly Func<IDbConnection> _connFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    public BaseUnitOfWork(Func<IDbConnection> connFactory)
    {
        _connFactory = connFactory ?? throw new ArgumentNullException(nameof(connFactory));
    }

    public IDbConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                _connection = _connFactory();
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
            }
            return _connection;
        }
    }

    public IDbTransaction? Transaction => _transaction;

    public Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        // Dapper uses ADO.NET transactions; BeginTransaction is synchronous.
        EnsureConnectionOpen();
        _transaction = Connection.BeginTransaction(level);
        return Task.CompletedTask;
    }

    public Task CommitAsync()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        if (_transaction != null)
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
        return Task.CompletedTask;
    }

    private void EnsureConnectionOpen()
    {
        if (Connection.State != ConnectionState.Open)
            Connection.Open();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _transaction?.Dispose();
        _transaction = null;

        if (_connection != null)
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }
}
