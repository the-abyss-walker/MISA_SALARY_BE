using Dapper;
using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;
using System.Data;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class SalaryCompositionRepository: BaseRepository<SalaryComposition, int>,
    ISalaryCompositionRepository
{
    public SalaryCompositionRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
        
    }

    public async Task<PaginationResult<SalaryComposition>> FilterPaginationAsync(SalaryCompositionParameter parameter)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        
        var multi = await connection.QueryMultipleAsync(
            "proc_salary_composition_filter_pagination",
            new
            {
                p_page_size = parameter.PageSize,
                p_page_index = parameter.PageIndex,
                p_search_query = parameter.Query,
                p_status = parameter.Status,
                p_org_unit_ids = parameter.OrganizationUnitIds != null && parameter.OrganizationUnitIds.Any()
                    ? string.Join(",", parameter.OrganizationUnitIds)
                    : null
            },
            commandType: CommandType.StoredProcedure);

        var totalCount = await multi.ReadFirstAsync<int>();
        var items = (await multi.ReadAsync<SalaryComposition>()).ToList();

        return PaginationResult<SalaryComposition>.Create(totalCount, items);
    }

    public async Task<bool> UpdateSalaryCompositionStatusAsync(int id, Status status)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryComposition>();

        var (keyColumnName, _) = _entityAttributeValues.GetKeyColumnNameAndPropertyName<SalaryComposition>();

        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryComposition>(addKey: true);

        var statusColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryComposition.Status)).Key;
        if (string.IsNullOrWhiteSpace(statusColumn))
        {
            statusColumn = "salary_composition_status";
        }

        var commandText = $"""
                          UPDATE {tableName}
                          SET {statusColumn} = @Status
                          WHERE {keyColumnName} = @Id;
                          """;

        var parameters = new DynamicParameters();
        parameters.Add("@Status", (int)status);
        parameters.Add("@Id", id);

        var rows = await connection.ExecuteAsync(commandText, parameters);
        return rows > 0;
    }

    public async Task<bool> UpdateSalaryCompositionListStatusAsync(IEnumerable<int> ids, Status status)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryComposition>();
        var (keyColumnName, _) = _entityAttributeValues.GetKeyColumnNameAndPropertyName<SalaryComposition>();
        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryComposition>(addKey: true);
        var statusColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryComposition.Status)).Key;
        if (string.IsNullOrWhiteSpace(statusColumn))
        {
            statusColumn = "salary_composition_status";
        }
        var commandText = $"""
                          UPDATE {tableName}
                          SET {statusColumn} = @Status
                          WHERE {keyColumnName} IN @Ids;
                          """;
        var parameters = new DynamicParameters();
        parameters.Add("@Status", (int)status);
        parameters.Add("@Ids", ids);
        var rows = await connection.ExecuteAsync(commandText, parameters);
        return rows > 0;
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryComposition>();
        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryComposition>(addKey: true);
        var codeColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryComposition.SalaryCompositionCode)).Key;
        if (string.IsNullOrWhiteSpace(codeColumn))
        {
            codeColumn = "salary_composition_code";
        }
        var commandText = $"""
                           SELECT COUNT(1)
                           FROM {tableName}
                           WHERE {codeColumn} = @Code;
                           """;
        var parameters = new DynamicParameters();
        parameters.Add("@Code", code);
        var count = await connection.ExecuteScalarAsync<int>(commandText, parameters);
        return count > 0;
    }

    public async Task<bool> AddRangeAsync(IEnumerable<SalaryComposition> entities)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryComposition>();
        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryComposition>(addKey: false);
        var columnNames = string.Join(", ", columnMappings.Keys);
        var parameterNames = string.Join(", ", columnMappings.Values.Select(c => "@" + c));
        var commandText = $"""
                          INSERT INTO {tableName} ({columnNames})
                          VALUES ({parameterNames});
                          """;
        var rows = await connection.ExecuteAsync(commandText, entities);
        return rows > 0;
    }

    public async Task<bool> UpdateRangeAsync(IEnumerable<SalaryComposition> entities)
    {
        if (entities == null) return false;
        var entityList = entities.ToList();
        if (entityList.Count == 0) return false;

        await using var connection = await _dataSource.OpenConnectionAsync();

        var tableName = _entityAttributeValues.GetTableName<SalaryComposition>();

        var (keyColumnName, keyPropertyName) = _entityAttributeValues
            .GetKeyColumnNameAndPropertyName<SalaryComposition>();

        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryComposition>(addKey: true);

        // Build SET clause excluding primary key column
        var setColumns = columnMappings
            .Where(cm => !string.Equals(cm.Value, keyPropertyName, StringComparison.OrdinalIgnoreCase))
            .Select(cm => $"{cm.Key} = @{cm.Value}");
        var setClause = string.Join(", ", setColumns);

        // Prefer updating by code when available (to match system items), otherwise fallback to key column
        var codeColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryComposition.SalaryCompositionCode)).Key;

        string whereClause;
        if (!string.IsNullOrWhiteSpace(codeColumn))
        {
            whereClause = $"{codeColumn} = @SalaryCompositionCode";
        }
        else
        {
            whereClause = $"{keyColumnName} = @{keyPropertyName}";
        }

        var commandText = $"""
                          UPDATE {tableName}
                          SET {setClause}
                          WHERE {whereClause};
                          """;

        var rows = await connection.ExecuteAsync(commandText, entityList);
        return rows > 0;
    }
}
