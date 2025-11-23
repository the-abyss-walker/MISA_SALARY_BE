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
public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    protected readonly MySqlDataSource _dataSource;
    protected readonly IEntityAttributeValues _entityAttributeValues;

    protected BaseRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues)
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
    /// Lấy các bản ghi theo khóa chính
    /// </summary>
    /// <param name="ids">Giá trị các khóa chính cần tìm</param>
    /// <returns>Danh sác các bản ghi</returns>
    public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids)
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
                           WHERE {keyColumnName} IN @{keyPropertyName};
                           """;
        var parameters = new DynamicParameters();
        parameters.Add($"@{keyPropertyName}", ids);
        return await connection.QueryAsync<TEntity>(commandText, parameters);
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
        var count = await connection.ExecuteAsync(commandText, entity);
        return count > 0;
    }

    /// <summary>
    /// Cập nhật từng phần (partial update) theo DTO có chứa khóa chính.
    /// Chỉ update các thuộc tính có giá trị khác null.
    /// </summary>
    /// <typeparam name="TUpdate">DTO update chứa khóa chính</typeparam>
    /// <param name="updateRequest">Đối tượng update</param>
    public async Task<bool> UpdatePartialAsync<TUpdate>(TUpdate updateRequest)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var tableName = _entityAttributeValues.GetTableName<TEntity>();
        var columnMappings = _entityAttributeValues.GetColumnMappings<TEntity>(addKey: true);

        var (keyColumnName, keyPropertyName) =
            _entityAttributeValues.GetKeyColumnNameAndPropertyName<TEntity>();

        // Lấy thuộc tính khóa chính từ DTO update
        var keyProp = typeof(TUpdate).GetProperty(keyPropertyName);
        if (keyProp is null)
            return false;

        var idValue = keyProp.GetValue(updateRequest);
        if (idValue is null)
            return false;

        var parameters = new DynamicParameters();
        parameters.Add($"@{keyPropertyName}", idValue);

        // Lấy các thuộc tính khác null cần update
        var requestProperties = typeof(TUpdate)
            .GetProperties()
            .Where(p => p.Name != keyPropertyName) // bỏ qua khóa chính
            .Where(p => p.GetValue(updateRequest) != null)
            .ToList();

        // Không có gì để update
        if (requestProperties.Count == 0)
            return false;

        var setClauses = new List<string>();

        foreach (var prop in requestProperties)
        {
            // prop.Name phải tồn tại trong mapping entity
            if (!columnMappings.ContainsValue(prop.Name))
                continue;

            var columnName = columnMappings.First(x => x.Value == prop.Name).Key;

            setClauses.Add($"{columnName} = @{prop.Name}");
            parameters.Add($"@{prop.Name}", prop.GetValue(updateRequest));
        }

        if (setClauses.Count == 0)
            return false;

        var commandText = $"""
                      UPDATE {tableName}
                      SET {string.Join(", ", setClauses)}
                      WHERE {keyColumnName} = @{keyPropertyName};
                      """;

        var affected = await connection.ExecuteAsync(commandText, parameters);
        return affected > 0;
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
                          WHERE {keyColumnName} = @{keyPropertyName};
                          """;
        var parameters = new DynamicParameters();
        parameters.Add($"@{keyPropertyName}", id);
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

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageSize, int pageIndex)
    {
        // sanitize inputs
        var safePageSize = pageSize <= 0 ? 10 : pageSize;
        var safePageIndex = pageIndex <= 0 ? 1 : pageIndex;
        var offset = (safePageIndex - 1) * safePageSize;

        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();

        var columnMappings = _entityAttributeValues
            .GetColumnMappings<TEntity>(addKey: true);

        var aliasedColumns = _entityAttributeValues
            .GetFormattedStringFromColumnMappings<TEntity>(columnMappings, "{0} AS {1}");

        var commandText = $"""
                           SELECT COUNT(1) FROM {tableName};
                           SELECT {aliasedColumns} FROM {tableName}
                           LIMIT @Offset, @PageSize;
                           """;

        var parameters = new DynamicParameters();
        parameters.Add("@Offset", offset);
        parameters.Add("@PageSize", safePageSize);

        using var multi = await connection.QueryMultipleAsync(commandText, parameters);
        var total = await multi.ReadFirstAsync<int>();
        var items = await multi.ReadAsync<TEntity>();
        return (items, total);
    }

    public async Task<int> BulkDeleteAsync(IEnumerable<TKey> ids)
    {
        if (ids == null) return 0;
        var idList = ids.ToList();
        if (idList.Count == 0) return 0;

        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<TEntity>();
        var (keyColumnName, _) = _entityAttributeValues
            .GetKeyColumnNameAndPropertyName<TEntity>();

        var commandText = $"""
                          DELETE FROM {tableName}
                          WHERE {keyColumnName} IN @Ids;
                          """;

        var parameters = new DynamicParameters();
        parameters.Add("@Ids", idList);

        return await connection.ExecuteAsync(commandText, parameters);
    }
}