using FluentValidation;
using MISA.Salary.Domain.Enums;
using ValueType = MISA.Salary.Domain.Enums.ValueType;

namespace MISA.Salary.Application.Commons.Models.SalaryComposition;
public class SalaryCompositionUpdateRequest
{
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
    public FormulaCompositionType? FormulaCompositionType { get; set; }
}

public class SalaryCompositionUpdateRequestValidator : AbstractValidator<SalaryCompositionUpdateRequest>
{
    public SalaryCompositionUpdateRequestValidator()
    {
        RuleFor(x => x.SalaryCompositionName)
            .NotEmpty().WithMessage("Tên thành phần lương không được để trống.")
            .MaximumLength(255).WithMessage("Tên thành phần lương không được vượt quá 255 ký tự.");
        RuleFor(x => x.SalaryCompositionCode)
            .NotEmpty().WithMessage("Mã thành phần lương không được để trống.")
            .MaximumLength(50).WithMessage("Mã thành phần lương không được vượt quá 50 ký tự.");
        RuleFor(x => x.ValueType)
            .IsInEnum().WithMessage("Kiểu giá trị không hợp lệ.");
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Trạng thái không hợp lệ.");
    }
}
