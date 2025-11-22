using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;

namespace MISA.Salary.Domain.Repositories;
public interface ISalaryCompositionSystemRepository : IBaseRepository<SalaryCompositionSystem, int>
{
    Task<PaginationResult<SalaryCompositionSystem>> FilterPaginationAsync(
        SalaryCompositionSystemParameter parameter);
    Task<bool> RemoveFromSystemCompositionsAsync(IEnumerable<int> id);
    Task<SalaryCompositionSystem> GetByCodeAsync(string salaryCompositionCode);
    Task<bool> ExistCompositionCode(string salaryCompositionCode);
}
