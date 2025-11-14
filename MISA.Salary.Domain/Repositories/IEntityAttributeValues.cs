namespace MISA.Salary.Domain.Repositories;
public interface IEntityAttributeValues
{
    string GetTableName<TEntity>();
    Dictionary<string, string> GetColumnMappings<TEntity>(bool addKey = true);
    string GetFormattedStringFromColumnMappings<TEntity>(Dictionary<string, string> columnMapping,
        string formatString);
    (string?, string) GetKeyColumnNameAndPropertyName<TEntity>();
}
