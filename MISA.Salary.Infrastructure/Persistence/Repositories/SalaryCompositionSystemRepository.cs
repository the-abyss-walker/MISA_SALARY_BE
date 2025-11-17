using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class SalaryCompositionSystemRepository : BaseRepository<SalaryCompositionSystem, int>,
    ISalaryCompositionSystemRepository
{
    public SalaryCompositionSystemRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
    }
}
