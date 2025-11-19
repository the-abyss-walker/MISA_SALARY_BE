using MISA.Salary.Application.Commons.Mapping;
using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Infrastructure.Persistence.Repositories;

namespace MISA.Salary.Application.UseCases.Implements;
public class SalaryCompositionService(ISalaryCompositionRepository salaryCompositionRepository) 
    : ISalaryCompostionService
{
    public async Task<Result<PaginationResult<SalaryCompositionResponse>>> GetAllSalaryComposition(int pageSize, int pageIndex)
    {
        var (entities, totalCount) = await salaryCompositionRepository.GetPagedAsync(pageSize, pageIndex);

        var items = entities
            .Select(SalaryCompositionMapping.ToSalaryCompositionResponse)
            .ToList();

        var pagination = PaginationResult<SalaryCompositionResponse>.Create(pageSize <= 0 ? 10 : pageSize,
                                                                           pageIndex <= 0 ? 1 : pageIndex,
                                                                           totalCount,
                                                                           items);

        return Result<PaginationResult<SalaryCompositionResponse>>.Success(pagination);
    }

    public async Task<Result<SalaryCompositionResponse>> GetSalaryCompositionById(int salaryCompositionId)
    {
        var entity = await salaryCompositionRepository.GetByIdAsync(salaryCompositionId);
        if (entity == null)
        {
            return Result<SalaryCompositionResponse>.Failure();
        }
        var res = SalaryCompositionMapping.ToSalaryCompositionResponse(entity);
        return Result<SalaryCompositionResponse>.Success(res);
    }

    public async Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request)
    {
        // Map request to entity
        var salaryComposition = SalaryCompositionMapping.ToSalaryCompositionEntity(request);
        // Save to database
        await salaryCompositionRepository.AddAsync(salaryComposition);

        var res = SalaryCompositionMapping.ToSalaryCompositionResponse(salaryComposition);
        return Result<SalaryCompositionResponse>.Success(res, 201);
    }

    public async Task<Result<SalaryCompositionResponse>> UpdateSalaryComposition(SalaryCompositionUpdateRequest request)
    {
        var existingEntity = await salaryCompositionRepository.GetByIdAsync(request.Id);
        if (existingEntity == null)
        {
            return Result<SalaryCompositionResponse>.Failure();
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
            return Result.Failure();
        }

        if (existingEntity.IsDefault)
        {
            return Result.Failure(400, new Error("CannotDeleteDefault", "Cannot delete default salary composition."));
        }
        await salaryCompositionRepository.DeleteAsync(salaryCompositionId);
        return Result.Success(204);
    }

    public async Task<Result> BulkDeleteSalaryCompositions(IEnumerable<int> salaryCompositionIds)
    {
        var idsList = salaryCompositionIds.ToList();
        var defaultEntities = new List<int>();
        foreach (var id in idsList)
        {
            var entity = await salaryCompositionRepository.GetByIdAsync(id);
            if (entity != null && entity.IsDefault)
            {
                defaultEntities.Add(id);
            }
        }
        if (defaultEntities.Count != 0)
        {
            return Result.Failure(400, new Error("CannotDeleteDefault", "Cannot delete default salary compositions."));
        }
        var deletedCount = await salaryCompositionRepository.BulkDeleteAsync(idsList);
        if (deletedCount == idsList.Count)
        {
            return Result.Success(204);
        }
        else
        {
            return Result.Failure(400, new Error("PartialDeletion", "Some salary compositions could not be deleted."));
        }
    }
}
