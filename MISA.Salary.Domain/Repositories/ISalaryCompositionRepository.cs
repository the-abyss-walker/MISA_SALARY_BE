using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Domain.Repositories;
using MISA.Salary.Infrastructure.Common;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;

public interface ISalaryCompositionRepository : IBaseRepository<SalaryComposition, int>
{
    Task<PaginationResult<SalaryComposition>> FilterPaginationAsync(SalaryCompositionParameter parameter);
    Task<bool> UpdateSalaryCompositionStatus(int id, Status status);
}
