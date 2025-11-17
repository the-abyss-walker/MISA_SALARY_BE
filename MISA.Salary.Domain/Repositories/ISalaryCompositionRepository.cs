using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;

public interface ISalaryCompositionRepository : IBaseRepository<SalaryComposition, int>
{
}