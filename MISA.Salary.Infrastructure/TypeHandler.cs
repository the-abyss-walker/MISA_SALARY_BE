using Dapper;
using System.Data;
using System.Text.Json;

namespace MISA.Salary.Infrastructure;
public class JsonListStringHandler : SqlMapper.TypeHandler<List<string>>
{
    public override List<string>? Parse(object value)
    {
        if (value is string json)
        {
            return JsonSerializer.Deserialize<List<string>>(json);
        }
        return new List<string>();
    }

    public override void SetValue(IDbDataParameter parameter, List<string>? value)
    {
        parameter.Value = JsonSerializer.Serialize(value ?? new List<string>());
        parameter.DbType = System.Data.DbType.String;
    }
}

