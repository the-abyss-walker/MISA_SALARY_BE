using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class SalaryCompositionRepository: BaseRepository<SalaryComposition, int>,
    ISalaryCompositionRepository
{
    public SalaryCompositionRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
    }
}
