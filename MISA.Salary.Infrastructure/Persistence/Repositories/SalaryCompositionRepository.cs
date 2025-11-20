using Dapper;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Domain.Repositories;
using MISA.Salary.Infrastructure.Common;
using MySqlConnector;
using System.Net.WebSockets;

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
            },
            commandType: System.Data.CommandType.StoredProcedure);

        var totalCount = await multi.ReadFirstAsync<int>();
        var items = (await multi.ReadAsync<SalaryComposition>()).ToList();

        return PaginationResult<SalaryComposition>.Create(totalCount, items);
    }

    public async Task<bool> UpdateSalaryCompositionStatus(int id, Status status)
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
}
