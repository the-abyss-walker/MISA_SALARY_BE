using MISA.Salary.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Salary.Domain.Entitites;

[Table("pa_organization_unit")]
public class OrganizationUnit : IEntity<int>, ISoftDelete
{
    [Key]
    [Column("organization_unit_id")]
    public int Id { get; set; }
    
    [Column("organization_unit_name")]
    public string OrganizationName { get; set; } = string.Empty;

    [Column("parent_id")]
    public string ParentId { get; set; } = string.Empty;

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}
