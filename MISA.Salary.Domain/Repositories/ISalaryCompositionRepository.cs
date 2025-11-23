using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Domain.Repositories;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;

public interface ISalaryCompositionRepository : IBaseRepository<SalaryComposition, int>
{
    Task<PaginationResult<SalaryComposition>> FilterPaginationAsync(SalaryCompositionParameter parameter);
    Task<bool> UpdateSalaryCompositionStatusAsync(int id, Status status);
    Task<bool> UpdateSalaryCompositionListStatusAsync(IEnumerable<int> ids, Status status);
    Task<bool> AddRangeAsync(IEnumerable<SalaryComposition> entities);
    Task<bool> ExistsByCodeAsync(string code);
    Task<bool> UpdateRangeAsync(IEnumerable<SalaryComposition> entities);
}
