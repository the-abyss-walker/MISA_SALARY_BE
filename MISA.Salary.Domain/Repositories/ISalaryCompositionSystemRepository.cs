using MISA.Salary.Domain.Entitites;

namespace MISA.Salary.Domain.Repositories;
public interface ISalaryCompositionSystemRepository : IBaseRepository<SalaryCompositionSystem, int>
{
    public Task<bool> RemoveFromSystemCompositionsAsync(int id);
}
