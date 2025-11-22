using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface ISalaryCompositionSystemService
{
    Task<Result<PaginationResult<SalaryCompositionSystem>>> FilterSalaryCompositionSystemPaginationAsync(
        SalaryCompositionSystemParameter parameter);
    Task<Result<SalaryCompositionSystem>> ExistCompositionCode(string salaryCompositionCode);
}
