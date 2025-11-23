using MISA.Salary.Domain.Enums;
using ValueType = MISA.Salary.Domain.Enums.ValueType;

namespace MISA.Salary.Application.Commons.Models.SalaryCompositionSystem;
public class SalaryCompositionSystemResponse
{
    public int Id { get; set; }
    public string SalaryCompositionSystemName { get; set; } = string.Empty;
    public string SalaryCompositionSystemCode { get; set; } = string.Empty;
    public CompositionType CompositionType { get; set; }
    public CompositionNature CompositionNature { get; set; }
    public bool? Taxable { get; set; }
    public bool? TaxDeduction { get; set; }
    public string? QuotaFormula { get; set; }
    public string? Formula { get; set; }
    public ValueType ValueType { get; set; }
    public string? Description { get; set; }
    public OptionShowPaycheck? OptionShowPaycheck { get; set; }
    public bool IsUsed { get; set; }
}
