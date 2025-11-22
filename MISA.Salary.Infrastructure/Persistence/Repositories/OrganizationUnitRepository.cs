using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure.Persistence.Repositories;
public class OrganizationUnitRepository : BaseRepository<OrganizationUnit, int>, IOrganizationUnitRepository
{
    public OrganizationUnitRepository(MySqlDataSource dataSource, IEntityAttributeValues entityAttributeValues) 
        : base(dataSource, entityAttributeValues)
    {
    }
}
