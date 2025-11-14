using MISA.Salary.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Salary.Domain.Entitites;

[Table("pa_organization")]
public class Organization : IEntity<int>
{
    [Key]
    [Column("organization_id")]
    public int Id { get; set; }

    [Column("organization_name")]
    public string OrganizationName { get; set; } = string.Empty;
}
