using MISA.Salary.Domain.Abstract;

namespace MISA.Salary.Domain.Entitites;
public class SalaryComposition : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string SalaryCompositionCode { get; set; }
    public string SalaryCompositionName { get; set; }
    public Guid SalaryTypeId { get; set; }
    public decimal Amount { get; set; }
    public bool IsTaxable { get; set; }
}
