using Dapper;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class OrganizationUnitRepository : BaseRepository<OrganizationUnit, int>, IOrganizationUnitRepository
{
    public OrganizationUnitRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
    }

    public async Task<OrganizationUnit?> GetRootUnitAsync()
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var tableName = _entityAttributeValues.GetTableName<OrganizationUnit>();

        var columnMappings = _entityAttributeValues
            .GetColumnMappings<OrganizationUnit>(addKey: true);

        var aliasedColumns = _entityAttributeValues
            .GetFormattedStringFromColumnMappings<OrganizationUnit>(columnMappings, "{0} AS {1}");

        var parentIdColumn = columnMappings
            .FirstOrDefault(cm => cm.Value == nameof(OrganizationUnit.ParentId)).Key;
        if (string.IsNullOrWhiteSpace(parentIdColumn))
        {
            parentIdColumn = "parent_id";
        }

        var commandText = $"""
                            SELECT {aliasedColumns}
                            FROM {tableName}
                            WHERE {parentIdColumn} IS NULL OR {parentIdColumn} = 0
                            """;

        return await connection.QuerySingleOrDefaultAsync<OrganizationUnit>(commandText);
    }

}
