using MISA.Salary.Contract.Exceptions;
using MISA.Salary.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class EntityAttributeValues : IEntityAttributeValues
{
    /// <summary>
    /// Lấy tên bảng từ entity
    /// </summary>
    /// <typeparam name="TEntity">Loại entity</typeparam>
    /// <returns>Tên của bảng</returns>
    /// <exception cref="InvalidOperationException">Trả về lỗi khi không có tên bảng</exception>
    /// Created by: VoVo (10/11/2025)
    public string GetTableName<TEntity>()
    {
        var attributes = typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), true);

        if (attributes.Length == 0)
        {
            throw new ConfigurationException($"The entity {typeof(TEntity).Name} does not have a Table attribute.");
        }

        var tableAttribute = (TableAttribute)attributes[0];

        return tableAttribute.Name;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="addKey"></param>
    /// <returns></returns>
    public Dictionary<string, string> GetColumnMappings<TEntity>(bool addKey = true)
    {
        var columnMappings = new Dictionary<string, string>();
        var properties = typeof(TEntity).GetProperties();

        foreach (var property in properties)
        {
            if (property.GetCustomAttributes(typeof(ColumnAttribute), true)
                    .FirstOrDefault() is not ColumnAttribute columnAttribute) continue;
            if (!addKey && property.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0) continue;
            if (columnAttribute.Name != null)
            {
                columnMappings.Add(columnAttribute.Name, property.Name);
            }
        }
        return columnMappings;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="columnMapping"></param>
    /// <param name="formatString"></param>
    /// <returns></returns>
    public string GetFormattedStringFromColumnMappings<TEntity>(Dictionary<string, string> columnMapping, string formatString)
    {
        var res = string.Join(", ", columnMapping
            .Select(columnNamePropName =>
                string.Format(formatString, columnNamePropName.Key, columnNamePropName.Value)));

        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public (string?, string) GetKeyColumnNameAndPropertyName<TEntity>()
    {
        var properties = typeof(TEntity).GetProperties();
        foreach (var property in properties)
        {
            if (property.GetCustomAttributes(typeof(KeyAttribute), true)
                    .FirstOrDefault() is not KeyAttribute) continue;
            var columnAttribute = property.GetCustomAttributes(typeof(ColumnAttribute), true)
                .FirstOrDefault() as ColumnAttribute;
            return (columnAttribute?.Name, property.Name);
        }

        throw new ConfigurationException("");
    }
}
