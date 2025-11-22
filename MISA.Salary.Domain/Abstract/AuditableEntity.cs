using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Salary.Domain.Abstract;
public abstract class AuditableEntity
{
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? ModifiedAt { get; set; }
}
