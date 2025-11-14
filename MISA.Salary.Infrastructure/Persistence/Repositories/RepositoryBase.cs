using Dapper;
using MISA.Salary.Domain.Abstract;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
/// <summary>
/// Lớp cơ sở cho Repository dùng để thao tác dữ liệu với MySQL.
/// Hỗ trợ các thao tác CRUD (Create, Read, Update, Delete) cơ bản.
/// </summary>
/// <typeparam name="TEntity">Kiểu thực thể (Entity) tương ứng với bảng trong CSDL</typeparam>
/// <typeparam name="TKey">Kiểu dữ liệu của khóa chính (Primary Key)</typeparam>
public abstract class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    private readonly MySqlDataSource _dataSource;
    private readonly IEntityAttributeValues _entityAttributeValues;

    protected RepositoryBase(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues)
    {
        _entityAttributeValues = entityAttributeValues;
        _dataSource = dataSource;
    }

    /// <summary>
    /// Lấy toàn bộ dữ liệu từ bảng tương ứng trong cơ sở dữ liệu.
    /// </summary>
    /// <returns>Danh sách tất cả các bản ghi thuộc entity</returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();

        var columnMappings = _entityAttributeValues
            .GetColumnMappings<TEntity>(addKey: true);// Đưa Column và Property vào dictionary

        var aliasedColumns = _entityAttributeValues
            .GetFormattedStringFromColumnMappings<TEntity>(columnMappings, "{0} AS {1}");
        // Định dạng chuỗi cột với alias

        var commandText = $"SELECT {aliasedColumns} FROM {tableName};";
        return await connection.QueryAsync<TEntity>(commandText);
    }

    /// <summary>
    /// Lấy một bản ghi duy nhất theo khóa chính.
    /// </summary>
    /// <param name="id">Giá trị của khóa chính cần tìm</param>
    /// <returns>Thực thể TEntity nếu tồn tại, ngược lại trả về null</returns>
    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();

        var columnMappings = _entityAttributeValues
            .GetColumnMappings<TEntity>(addKey: true);

        var aliasedColumns = _entityAttributeValues
            .GetFormattedStringFromColumnMappings<TEntity>(columnMappings, "{0} AS {1}");

        var (keyColumnName, keyPropertyName) = _entityAttributeValues
            .GetKeyColumnNameAndPropertyName<TEntity>();

        var commandText = $"""
                           SELECT {aliasedColumns} 
                           FROM {tableName} 
                           WHERE {keyColumnName} = @{keyPropertyName};
                           """;

        var parameters = new DynamicParameters();
        parameters.Add($"@{keyPropertyName}", id);
        return await connection.QuerySingleOrDefaultAsync<TEntity>(commandText, parameters);
    }

    /// <summary>
    /// Thêm mới một bản ghi vào bảng tương ứng.
    /// </summary>
    /// <param name="entity">Thực thể cần thêm</param>
    /// <returns>True nếu thêm thành công, ngược lại False</returns>
    public async Task<bool> AddAsync(TEntity entity)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();

        var columnMappings = _entityAttributeValues
            .GetColumnMappings<TEntity>(addKey: true);

        var commandText = $"""
                          INSERT INTO {tableName}
                          ({string.Join(", ", columnMappings.Keys)})
                          VALUES ({string.Join(", ", columnMappings.Values.Select(
                              name => $"@{name}"))});
                          """;
        return await connection.ExecuteAsync(commandText, entity) > 0;
    }

    /// <summary>
    /// Cập nhật dữ liệu của một bản ghi dựa trên khóa chính.
    /// </summary>
    /// <param name="entity">Thực thể chứa dữ liệu cần cập nhật</param>
    /// <returns>True nếu cập nhật thành công, ngược lại False</returns>
    public async Task<bool> UpdateAsync(TEntity entity)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();

        var columnMappings = _entityAttributeValues
            .GetColumnMappings<TEntity>(addKey: true);

        var (keyColumnName, keyPropertyName) = _entityAttributeValues
            .GetKeyColumnNameAndPropertyName<TEntity>();

        var aliasedColumns = _entityAttributeValues
            .GetFormattedStringFromColumnMappings<TEntity>(columnMappings, "{0} = @{1}");

        var commandText = $"""
                          UPDATE {tableName}
                          SET {aliasedColumns}
                          WHERE {keyColumnName} = @{keyPropertyName};
                          """;
        return await connection.ExecuteAsync(commandText, entity) > 0;
    }

    /// <summary>
    /// Xóa một bản ghi dựa trên khóa chính.
    /// </summary>
    /// <param name="id">Giá trị khóa chính của bản ghi cần xóa</param>
    public async Task DeleteAsync(TKey id)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();

        var (keyColumnName, keyPropertyName) = _entityAttributeValues
            .GetKeyColumnNameAndPropertyName<TEntity>();

        var commandText = $"""
                          DELETE FROM {tableName}
                          WHERE {keyPropertyName} = @{keyColumnName};
                          """;
        var parameters = new DynamicParameters();
        parameters.Add($"@{keyColumnName}", id);
        await connection.ExecuteAsync(commandText, parameters);
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();
        var (keyColumnName, keyPropertyName) = _entityAttributeValues
            .GetKeyColumnNameAndPropertyName<TEntity>();

        var commandText = $"""
                           SELECT COUNT(1)
                           FROM {tableName}
                           WHERE {keyColumnName} = @{keyPropertyName};
                           """;

        var parameters = new DynamicParameters();
        parameters.Add($"@{keyPropertyName}", id);

        var count = await connection.ExecuteScalarAsync<int>(commandText, parameters);
        return count > 0;
    }

}