using MISA.Salary.Domain.Entitites;

namespace MISA.Salary.Application.Commons.Models.SalaryComposition;
public class DefaultCompositionCheckResponse
{
    public List<SalaryCompositionResponse> DefaultComposition { get; set; } = [];
    public List<SalaryCompositionResponse> NormalComposition { get; set; } = [];
}
