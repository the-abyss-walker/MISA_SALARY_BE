using MISA.Salary.Domain.Abstract;
using MISA.Salary.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValueType = MISA.Salary.Domain.Enums.ValueType;

namespace MISA.Salary.Domain.Entitites;

[Table("pa_salary_composition_system")]
public class SalaryCompositionSystem : AuditableEntity, IEntity<int>
{
    [Key]
    [Column("salary_composition_system_id")]
    public int Id { get; set; }

    [Column("salary_composition_system_name")]
    public string SalaryCompositionSystemName { get; set; } = string.Empty;

    [Column("salary_composition_system_code")]
    public string SalaryCompositionSystemCode { get; set; } = string.Empty;

    [Column("salary_composition_system_type")]
    public CompositionType CompositionType { get; set; }

    [Column("salary_composition_system_nature")]
    public CompositionNature CompositionNature { get; set; }

    [Column("salary_composition_system_taxable")]
    public bool? Taxable { get; set; }

    [Column("salary_composition_system_tax_deduction")]
    public bool? TaxDeduction { get; set; }

    [Column("salary_composition_system_quota")]
    public string? QuotaFormula { get; set; }

    [Column("salary_composition_system_formula")]
    public string? Formula { get; set; }

    [Column("salary_composition_system_value_type")]
    public ValueType ValueType { get; set; }

    [Column("salary_composition_system_description")]
    public string? Description { get; set; }

    [Column("salary_composition_system_option_show_paycheck")]
    public OptionShowPaycheck? OptionShowPaycheck { get; set; }

    [Column("salary_composition_system_is_used")]
    public bool IsUsed { get; set; }
}
