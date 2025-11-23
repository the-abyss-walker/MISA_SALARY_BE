using Dapper;
using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;
using System.Net.WebSockets;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class SalaryCompositionSystemRepository : BaseRepository<SalaryCompositionSystem, int>,
    ISalaryCompositionSystemRepository
{
    public SalaryCompositionSystemRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
    }

    public async Task<PaginationResult<SalaryCompositionSystem>> FilterPaginationAsync(SalaryCompositionSystemParameter parameter)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var multi = await connection.QueryMultipleAsync(
            "proc_salary_composition_system_filter_pagination",
            new
            {
                p_page_size = parameter.PageSize,
                p_page_index = parameter.PageIndex,
                p_search_query = parameter.Query,
                p_composition_type = parameter.CompositionType,
            },
            commandType: System.Data.CommandType.StoredProcedure);
        var totalCount = await multi.ReadFirstAsync<int>();
        var items = (await multi.ReadAsync<SalaryCompositionSystem>()).ToList();
        return PaginationResult<SalaryCompositionSystem>.Create(totalCount, items);
    }

    public async Task<bool> RemoveFromSystemCompositionsAsync(IEnumerable<int> ids)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryCompositionSystem>();

        var (keyColumnName, _) = _entityAttributeValues.GetKeyColumnNameAndPropertyName<SalaryCompositionSystem>();

        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryCompositionSystem>(addKey: true);

        var isUsedColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryCompositionSystem.IsUsed)).Key;
        if (string.IsNullOrWhiteSpace(isUsedColumn))
        {
            isUsedColumn = "salary_composition_system_is_used";
        }

        var commandText = $"""
                          UPDATE {tableName}
                          SET {isUsedColumn} = 1
                          WHERE {keyColumnName} IN @Ids;
                          """;
        var parameters = new DynamicParameters();
        parameters.Add("@Ids", ids);

        var rows = await connection.ExecuteAsync(commandText, parameters);
        return rows > 0;
    }

    public async Task<bool> ExistCompositionCode(string salaryCompositionCode)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryCompositionSystem>();
        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryCompositionSystem>(addKey: false);
        var codeColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryCompositionSystem.SalaryCompositionSystemCode)).Key;
        var isUssedColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryCompositionSystem.IsUsed)).Key;
        if (string.IsNullOrWhiteSpace(codeColumn))
        {
            codeColumn = "salary_composition_system_code";
        }
        if (string.IsNullOrWhiteSpace(isUssedColumn))
        {
            isUssedColumn = "salary_composition_system_is_used";
        }
        var commandText = $"""
                          SELECT COUNT(1) 
                          FROM {tableName} 
                          WHERE 
                            {codeColumn} = @SalaryCompositionCode
                            AND {isUssedColumn} = 0;
                          """;
        var parameters = new DynamicParameters();
        parameters.Add("@SalaryCompositionCode", salaryCompositionCode);
        var count = await connection.ExecuteScalarAsync<int>(commandText, parameters);
        return count > 0;
    }

    public async Task<SalaryCompositionSystem> GetByCodeAsync(string salaryCompositionCode)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var tableName = _entityAttributeValues.GetTableName<SalaryCompositionSystem>();
        var columnMappings = _entityAttributeValues.GetColumnMappings<SalaryCompositionSystem>(addKey: true);
        var aliasedColumns = _entityAttributeValues
            .GetFormattedStringFromColumnMappings<SalaryCompositionSystem>(columnMappings, "{0} AS {1}");
        var codeColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(SalaryCompositionSystem.SalaryCompositionSystemCode)).Key;
        if (string.IsNullOrWhiteSpace(codeColumn))
        {
            codeColumn = "salary_composition_code";
        }
        var commandText = $"""
                          SELECT {aliasedColumns}
                          FROM {tableName} 
                          WHERE {codeColumn} = @SalaryCompositionCode;
                          """;
        var parameters = new DynamicParameters();
        parameters.Add("@SalaryCompositionCode", salaryCompositionCode);
        var result = await connection.QueryFirstOrDefaultAsync<SalaryCompositionSystem>(commandText, parameters);
        return result!;
    }
}
