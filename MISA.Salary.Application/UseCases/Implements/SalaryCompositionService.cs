using MISA.Salary.Application.Commons.Errors;
using MISA.Salary.Application.Commons.Mapping;
using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Application.Commons.Models.SalaryCompositionSystem;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Domain.Repositories;
using MISA.Salary.Infrastructure.Persistence.Repositories;

namespace MISA.Salary.Application.UseCases.Implements;
public class SalaryCompositionService(
    ISalaryCompositionRepository salaryCompositionRepository, 
    ISalaryCompositionSystemRepository salaryCompositionSystemRepository,
    IOrganizationUnitRepository organizationUnitRepository) 
    : ISalaryCompostionService
{
    public async Task<Result<IEnumerable<SalaryCompositionResponse>>> GetAllSalaryCompositionsAsync()
    {
        var entities = await salaryCompositionRepository.GetAllAsync();
        var res = entities.Select(SalaryCompositionMapping.ToSalaryCompositionResponse);
        return Result<IEnumerable<SalaryCompositionResponse>>.Success(res);
    }
    
    public async Task<Result<PaginationResult<SalaryComposition>>> FilterSalaryCompositionPaginationAsync(
        SalaryCompositionParameter parameter)
    {
        var res = await salaryCompositionRepository.FilterPaginationAsync(parameter);

        return Result<PaginationResult<SalaryComposition>>.Success(res);
    }

    public async Task<Result<SalaryCompositionResponse>> GetSalaryCompositionById(int salaryCompositionId)
    {
        var entity = await salaryCompositionRepository.GetByIdAsync(salaryCompositionId);
        if (entity == null)
        {
            return Result<SalaryCompositionResponse>.Failure(404, SalaryCompositionErrors.SalaryCompositionNotFound);
        }
        var res = SalaryCompositionMapping.ToSalaryCompositionResponse(entity);
        return Result<SalaryCompositionResponse>.Success(res);
    }

    public async Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request)
    {
        if (await salaryCompositionRepository.ExistsByCodeAsync(request.SalaryCompositionCode))
        {
            return Result<SalaryCompositionResponse>.Failure(400, SalaryCompositionErrors.SalaryCompositionCodeExists);
        }

        var organizationUnitNames = new List<string>();

        if (request.OrganizationUnitIds != null && request.OrganizationUnitIds.Count != 0)
        {
            foreach (var orgUnitId in request.OrganizationUnitIds)
            {
                var orgUnit = await organizationUnitRepository.GetByIdAsync(int.Parse(orgUnitId));
                if (orgUnit == null)
                {
                    return Result<SalaryCompositionResponse>.Failure(404, OrganizationUnitErrors.OrganizationUnitNotFound);
                }
                organizationUnitNames.Add(orgUnit != null ? orgUnit.OrganizationName : string.Empty);
            }
        }
        var salaryComposition = SalaryCompositionMapping.ToSalaryCompositionEntity(request);
        salaryComposition.OrganizationUnitNames = organizationUnitNames;
        await salaryCompositionRepository.AddAsync(salaryComposition);

        var res = SalaryCompositionMapping.ToSalaryCompositionResponse(salaryComposition);
        return Result<SalaryCompositionResponse>.Success(res, 201);
    }

    public async Task<Result<SalaryCompositionResponse>> UpdateSalaryComposition(SalaryCompositionUpdateRequest request)
    {
        var existingEntity = await salaryCompositionRepository.GetByIdAsync(request.Id);
        if (existingEntity == null)
        {
            return Result<SalaryCompositionResponse>.Failure(404, SalaryCompositionErrors.SalaryCompositionNotFound);
        }

        // Map request to entity
        var salaryComposition = SalaryCompositionMapping.ToSalaryCompositionEntity(request);
        // Update in database
        await salaryCompositionRepository.UpdateAsync(salaryComposition);
        var res = SalaryCompositionMapping.ToSalaryCompositionResponse(salaryComposition);
        return Result<SalaryCompositionResponse>.Success(res);
    }

    public async Task<Result> DeleteSalaryComposition(int salaryCompositionId)
    {
        var existingEntity = await salaryCompositionRepository.GetByIdAsync(salaryCompositionId);
        if (existingEntity == null)
        {
            return Result.Failure(404, SalaryCompositionErrors.SalaryCompositionNotFound);
        }

        if (existingEntity.IsDefault)
        {
            return Result.Failure(400, SalaryCompositionErrors.SalaryCompositionDefault);
        }
        await salaryCompositionRepository.DeleteAsync(salaryCompositionId);
        return Result.Success();
    }

    public async Task<Result<DefaultCompositionCheckResponse>> CheckDefaultComposition(IEnumerable<int> salaryCompositionIds)
    {
        var salaryCompositions = await salaryCompositionRepository.GetByIdsAsync(salaryCompositionIds);
        var salaryCompositionsResponese = salaryCompositions
            .Select(SalaryCompositionMapping.ToSalaryCompositionResponse)
            .ToList();

        var defaultEntities = new List<SalaryCompositionResponse>();
        var normalEntities = new List<SalaryCompositionResponse>();

        foreach (var entity in salaryCompositionsResponese)
        {
            if (entity is null)
                continue;

            if (!entity.IsDefault)
            {
                normalEntities.Add(entity);
            }
            else defaultEntities.Add(entity!);
        }

        var res = new DefaultCompositionCheckResponse
        {
            DefaultComposition = defaultEntities,
            NormalComposition = normalEntities
        };
        return res;
    }

    public async Task<Result> BulkDeleteSalaryCompositions(IEnumerable<int> salaryCompositionIds)
    {
        var deleteCount = await salaryCompositionRepository.BulkDeleteAsync(salaryCompositionIds);

        if (deleteCount == 0)
        {
            return Result.Failure(400, SalaryCompositionErrors.DeleteSalaryCompositionFailed);
        }

        return Result.Success();
    }

    public async Task<Result> UpdateSalaryCompositionStatus(int salaryCompositionId, Status status)
    {
        var existingEntity = await salaryCompositionRepository.GetByIdAsync(salaryCompositionId);
        if (existingEntity == null)
        {
            return Result.Failure(404, SalaryCompositionErrors.SalaryCompositionNotFound);
        }

        var updated = await salaryCompositionRepository.UpdateSalaryCompositionStatusAsync(salaryCompositionId, status);
        if (!updated)
        {
            return Result.Failure(400, SalaryCompositionErrors.SalaryCompositionUpdateStatusFailed);
        }

        return Result.Success();
    }

    public async Task<Result> UpdateListSalaryCompositionStatus(IEnumerable<int> salaryCompositionIds, Status status)
    {
        var update = await salaryCompositionRepository.UpdateSalaryCompositionListStatusAsync(salaryCompositionIds, status);

        if (!update)
        {
            return Result.Failure(400, SalaryCompositionErrors.SalaryCompositionUpdateStatusFailed);
        }

        return Result.Success();
    }

    public async Task<Result> CreateSalaryCompositionFromSystemAsync(IEnumerable<int> salaryCompositionSystemIds)
    {
        var salaryCompositionSystems = await salaryCompositionSystemRepository.GetByIdsAsync(salaryCompositionSystemIds);

        var salaryCompositions = salaryCompositionSystems.Select(SalaryCompositionMapping.ToSalaryCompositionEntity).ToList();
        
        if (await salaryCompositionRepository.AddRangeAsync(salaryCompositions))
        {
            await salaryCompositionSystemRepository.RemoveFromSystemCompositionsAsync(salaryCompositionSystemIds);
        }


        return Result.Success(201);
    }

    public async Task<Result<IEnumerable<SalaryCompositionSystemResponse>>> UpdateListSalaryCompositionFromSystemAsync(
        UpdateFromSystemRequest request)
    {
        // thành phần lương hệ thống
        var salaryCompositionSystems = await salaryCompositionSystemRepository.GetByIdsAsync(request.SalaryCompositionSystemIds);

        // thành phần lương hệ thống chuyển đổi sang response
        var salaryCompositionSystemsResponse = salaryCompositionSystems
            .Select(SalaryCompositionSystemMapping.ToSalaryCompositionSystemResponse);

        // thành phần lương hệ thống đã tồn tại trong thành phần lương
        var duplicated = new List<SalaryCompositionSystemResponse>();
        foreach (var system in salaryCompositionSystemsResponse)
        {
            var exists = await salaryCompositionRepository.ExistsByCodeAsync(system.SalaryCompositionSystemCode);
            if (exists)
            {
                duplicated.Add(system);
            }
        }

        if (request.IsAllowanceUpdate is null || request.IsAllowanceUpdate == false)
        {
            return Result<IEnumerable<SalaryCompositionSystemResponse>>.Success(duplicated);
        }

        // thành phần lương được cập nhật từ hệ thống
        var salaryCompositionsToUpdate = salaryCompositionSystems
            .Where(sc => duplicated.Any(r => r.Id == sc.Id))
            .Select(SalaryCompositionMapping.ToSalaryCompositionEntity);
        // cập nhật thành phần lương
        if (await salaryCompositionRepository.UpdateRangeAsync(salaryCompositionsToUpdate))
        {
            // id của thành phần lương hệ thống bị xóa
            var idsToRemove = duplicated.Select(r => r.Id);
            // xóa thành phần lương hệ thống sau khi cập nhật
            await salaryCompositionSystemRepository.RemoveFromSystemCompositionsAsync(idsToRemove);
        }

        // thành phần lương còn lại được thêm mới
        var salaryCompositionToInsert = salaryCompositionSystems
            .Where(sys => duplicated.All(d => d.Id != sys.Id))
            .Select(sys => sys.Id);

        await CreateSalaryCompositionFromSystemAsync(salaryCompositionToInsert);

        return Result<IEnumerable<SalaryCompositionSystemResponse>>.Success(null!);
    }
}
