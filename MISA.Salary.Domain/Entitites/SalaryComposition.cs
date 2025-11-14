using MISA.Salary.Domain.Abstract;
using MISA.Salary.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValueType = MISA.Salary.Domain.Enums.ValueType;

namespace MISA.Salary.Domain.Entitites;

[Table("pa_salary_compostion")]
public class SalaryComposition : IEntity<int>
{
    [Key]
    [Column("salary_composition_id")]
    public int Id { get; set; }

    [Column("salary_composition_name")]
    public string SalaryCompositionName { get; set; } = string.Empty;

    [Column("salary_composition_code")]
    public string SalaryCompositionCode { get; set; } = string.Empty;

    [Column("salary_composition_type")]
    public CompositionType? CompositionType { get; set; }

    [Column("salary_composition_nature")]
    public CompositionNature? CompositionNature { get; set; }

    [Column("salary_composition_taxable")]
    public bool? Taxable { get; set; }

    [Column("salary_composition_tax_deduction")]
    public bool? TaxDeduction { get; set; }

    [Column("salary_composition_quota")]
    public string? Quota { get; set; }

    [Column("salary_composition_formula")]
    public string? Formula { get; set; }

    [Column("salary_composition_value_type")]
    public ValueType ValueType { get; set; }

    [Column("salary_composition_desctiption")]
    public string? Description { get; set; }

    [Column("salary_composition_status")]
    public Status Status { get; set; }

    [Column("salary_composition_option_show_paycheck")]
    public OptionShowPaycheck? OptionShowPaycheck { get; set; }

    [Column("salary_composition_is_not_allow_delete")]
    public bool IsNotAllowDelete { get; set; }

    [Column("organization_unit_ids")]
    public List<string> OrganizationUnitIds { get; set; } = [];

    [Column("organization_unit_names")]
    public List<string> OrganizationUnitNames { get; set; } = [];

    [Column("salary_composition_is_default")]
    public bool IsDefault { get; set; }

    [Column("salary_composition_auto_sum_composition_code")]
    public string? AutoSumCompositionCode { get; set; }

    [Column("salary_composition_is_auto_sum_employee")]
    public bool IsAutoSumEmployee { get; set; }

    [Column("salary_composition_auto_sum_employee_type")]
    public AutoSumEmployeeType? AutoSumEmployeeType { get; set; }

    [Column("salary_composition_formula_composition_type")]
    public FormulaCompositionType? FormulaCompositionType { get; set; }
}
