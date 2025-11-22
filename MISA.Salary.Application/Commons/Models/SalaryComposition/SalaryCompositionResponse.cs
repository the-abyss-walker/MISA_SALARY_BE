using MISA.Salary.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using ValueType = MISA.Salary.Domain.Enums.ValueType;

namespace MISA.Salary.Application.Commons.Models.SalaryComposition;
public class SalaryCompositionResponse
{
    public int Id { get; set; }
    public string SalaryCompositionName { get; set; } = string.Empty;
    public string SalaryCompositionCode { get; set; } = string.Empty;
    public CompositionType? CompositionType { get; set; }
    public CompositionNature? CompositionNature { get; set; }
    public bool? Taxable { get; set; }
    public bool? TaxDeduction { get; set; }
    public string? Quota { get; set; }
    public string? Formula { get; set; }
    public ValueType ValueType { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public OptionShowPaycheck? OptionShowPaycheck { get; set; }
    public bool IsNotAllowDelete { get; set; }
    public List<string> OrganizationUnitIds { get; set; } = [];
    public List<string> OrganizationUnitNames { get; set; } = [];
    public bool IsDefault { get; set; }
    public string? AutoSumCompositionCode { get; set; }
    public bool IsAutoSumEmployee { get; set; }
    public AutoSumEmployeeType? AutoSumEmployeeType { get; set; }
    public AutoSumOrgLevel? AutoSumOrgLevel { get; set; }
    public FormulaCompositionType? FormulaCompositionType { get; set; }
}
