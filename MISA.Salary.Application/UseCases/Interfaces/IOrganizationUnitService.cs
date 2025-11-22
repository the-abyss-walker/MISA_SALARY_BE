using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface IOrganizationUnitService
{
    Task<Result<IEnumerable<OrganizationUnit>>> GetAllOrganizationUnitsAsync();
    Task<Result<OrganizationUnit>> GetOrganizationUnitByIdAsync(int organizationUnitId);
}
