using MISA.Salary.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Salary.Domain.Entitites;

[Table("pa_grid_config")]
public class GridConfig : IEntity<int>
{
    [Key]
    [Column("grid_config_id")]
    public int Id { get; set; }

    [Column("sub_system_code")]
    public string SubSystemCode { get; set; } = string.Empty;

    [Column("field_name")]
    public string FieldName { get; set; } = string.Empty;

    [Column("caption")]
    public string Caption { get; set; } = string.Empty;

    [Column("tooltip")]
    public string Tooltip { get; set; } = string.Empty;

    [Column("data_type")]
    public string DataType { get; set; } = string.Empty;

    [Column("is_visible")]
    public bool IsVisible { get; set; }

    [Column("is_lock")]
    public bool IsLock { get; set; }

    [Column("is_fixed")]
    public bool IsFixed { get; set; }
    
    [Column("is_default")]
    public bool IsDefault { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }
}
