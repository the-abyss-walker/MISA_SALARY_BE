using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;

namespace MISA.Salary.Application.Commons.Mapping;
public static class SalaryCompositionMapping
{
    public static SalaryCompositionResponse ToSalaryCompositionResponse(SalaryComposition salaryComposition)
    {
        return new SalaryCompositionResponse
        {
            Id = salaryComposition.Id,
            SalaryCompositionName = salaryComposition.SalaryCompositionName,
            SalaryCompositionCode = salaryComposition.SalaryCompositionCode,
            CompositionType = salaryComposition.CompositionType,
            CompositionNature = salaryComposition.CompositionNature,
            Taxable = salaryComposition.Taxable,
            TaxDeduction = salaryComposition.TaxDeduction,
            Quota = salaryComposition.Quota,
            Formula = salaryComposition.Formula,
            ValueType = salaryComposition.ValueType,
            Description = salaryComposition.Description,
            Status = salaryComposition.Status,
            OptionShowPaycheck = salaryComposition.OptionShowPaycheck,
            IsNotAllowDelete = salaryComposition.IsNotAllowDelete,
            OrganizationUnitIds = salaryComposition.OrganizationUnitIds,
            OrganizationUnitNames = salaryComposition.OrganizationUnitNames,
            IsDefault = salaryComposition.IsDefault,
            AutoSumCompositionCode = salaryComposition.AutoSumCompositionCode,
            IsAutoSumEmployee = salaryComposition.IsAutoSumEmployee,
            AutoSumEmployeeType = salaryComposition.AutoSumEmployeeType,
            AutoSumOrgLevel = salaryComposition.AutoSumOrgLevel,
            FormulaCompositionType = salaryComposition.FormulaCompositionType
        };
    }

    public static SalaryComposition ToSalaryCompositionEntity(this SalaryCompositionCreateRequest request)
    {
        return new SalaryComposition
        {
            SalaryCompositionName = request.SalaryCompositionName,
            SalaryCompositionCode = request.SalaryCompositionCode,
            CompositionType = request.CompositionType,
            CompositionNature = request.CompositionNature,
            Taxable = request.Taxable,
            TaxDeduction = request.TaxDeduction,
            Quota = request.Quota,
            Formula = request.Formula,
            ValueType = request.ValueType,
            Description = request.Description,
            Status = request.Status,
            OptionShowPaycheck = request.OptionShowPaycheck,
            IsNotAllowDelete = request.IsNotAllowDelete,
            OrganizationUnitIds = request.OrganizationUnitIds,
            IsDefault = request.IsDefault,
            AutoSumCompositionCode = request.AutoSumCompositionCode,
            IsAutoSumEmployee = request.IsAutoSumEmployee,
            AutoSumEmployeeType = request.AutoSumEmployeeType,
            AutoSumOrgLevel = request.AutoSumOrgLevel,
            FormulaCompositionType = request.FormulaCompositionType
        };
    }

    public static SalaryComposition ToSalaryCompositionEntity(this SalaryCompositionUpdateRequest request)
    {
        return new SalaryComposition
        {
            Id = request.Id,
            SalaryCompositionName = request.SalaryCompositionName,
            CompositionType = request.CompositionType,
            CompositionNature = request.CompositionNature,
            Taxable = request.Taxable,
            TaxDeduction = request.TaxDeduction,
            Quota = request.Quota,
            Formula = request.Formula,
            ValueType = request.ValueType,
            Description = request.Description,
            Status = request.Status,
            OptionShowPaycheck = request.OptionShowPaycheck,
            IsNotAllowDelete = request.IsNotAllowDelete,
            OrganizationUnitIds = request.OrganizationUnitIds,
            OrganizationUnitNames = request.OrganizationUnitNames,
            AutoSumCompositionCode = request.AutoSumCompositionCode,
            IsAutoSumEmployee = request.IsAutoSumEmployee,
            AutoSumEmployeeType = request.AutoSumEmployeeType,
            AutoSumOrgLevel = request.AutoSumOrgLevel,
            FormulaCompositionType = request.FormulaCompositionType
        };
    }

    public static SalaryComposition ToSalaryCompositionEntity(this SalaryCompositionCreateFromSystemRequest request)
    {
        return new SalaryComposition
        {
            SalaryCompositionName = request.SalaryCompositionName,
            SalaryCompositionCode = request.SalaryCompositionCode,
            CompositionType = request.CompositionType,
            CompositionNature = request.CompositionNature,
            Taxable = request.Taxable,
            TaxDeduction = request.TaxDeduction,
            Quota = request.QuotaFormula,
            Formula = request.Formula,
            ValueType = request.ValueType,
            Description = request.Description,
            Status = request.Status,
            OptionShowPaycheck = request.OptionShowPaycheck,
            IsNotAllowDelete = true 
        };
    }

    public static SalaryComposition ToSalaryCompositionEntity(this SalaryCompositionSystem request)
    {
        return new SalaryComposition
        {
            SalaryCompositionName = request.SalaryCompositionSystemName,
            SalaryCompositionCode = request.SalaryCompositionSystemCode,
            CompositionType = request.CompositionType,
            CompositionNature = request.CompositionNature,
            Taxable = request.Taxable,
            TaxDeduction = request.TaxDeduction,
            Quota = request.QuotaFormula,
            Formula = request.Formula,
            ValueType = request.ValueType,
            Description = request.Description,
            Status = Status.Following,
            OptionShowPaycheck = request.OptionShowPaycheck,
            IsDefault = true,
        };
    }
}
