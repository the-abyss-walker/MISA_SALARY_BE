using Dapper;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class SalaryCompositionSystemRepository : BaseRepository<SalaryCompositionSystem, int>,
    ISalaryCompositionSystemRepository
{
    public SalaryCompositionSystemRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
    }

    public async Task<bool> RemoveFromSystemCompositionsAsync(int id)
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
                          SET {isUsedColumn} = FALSE
                          WHERE {keyColumnName} = @Id;
                          """;
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        var rows = await connection.ExecuteAsync(commandText, parameters);
        return rows > 0;
    }
}
