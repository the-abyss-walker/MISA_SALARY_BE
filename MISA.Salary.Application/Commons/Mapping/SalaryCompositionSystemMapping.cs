using MISA.Salary.Application.Commons.Models.SalaryCompositionSystem;
using MISA.Salary.Domain.Entitites;

namespace MISA.Salary.Application.Commons.Mapping;
public class SalaryCompositionSystemMapping
{
    public static SalaryCompositionSystemResponse ToSalaryCompositionSystemResponse(
        SalaryCompositionSystem entity)
    {
        return new SalaryCompositionSystemResponse
        {
            Id = entity.Id,
            SalaryCompositionSystemName = entity.SalaryCompositionSystemName,
            SalaryCompositionSystemCode = entity.SalaryCompositionSystemCode,
            CompositionType = entity.CompositionType,
            CompositionNature = entity.CompositionNature,
            Taxable = entity.Taxable,
            TaxDeduction = entity.TaxDeduction,
            QuotaFormula = entity.QuotaFormula,
            Formula = entity.Formula,
            ValueType = entity.ValueType,
            Description = entity.Description,
            OptionShowPaycheck = entity.OptionShowPaycheck,
            IsUsed = entity.IsUsed
        };
    }
}
