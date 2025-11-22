using MISA.Salary.Application.Commons.Errors;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;

namespace MISA.Salary.Application.UseCases.Implements;
public class OrganizationUnitService : IOrganizationUnitService
{
    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    public OrganizationUnitService(IOrganizationUnitRepository organizationUnitRepository)
    {
        _organizationUnitRepository = organizationUnitRepository;
    }

    public async Task<Result<IEnumerable<OrganizationUnit>>> GetAllOrganizationUnitsAsync()
    {
        var organizationUnits = await _organizationUnitRepository.GetAllAsync();
        return Result<IEnumerable<OrganizationUnit>>.Success(organizationUnits);
    }

    public async Task<Result<OrganizationUnit>> GetOrganizationUnitByIdAsync(int organizationUnitId)
    {
        var organizationUnit = await _organizationUnitRepository.GetByIdAsync(organizationUnitId);
        if (organizationUnit == null)
        {
            return Result<OrganizationUnit>.Failure(404, OrganizationUnitErrors.OrganizationUnitNotFound);
        }
        return organizationUnit;
    }
}
