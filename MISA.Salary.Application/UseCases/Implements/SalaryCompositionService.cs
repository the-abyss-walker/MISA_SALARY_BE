using MISA.Salary.Application.Commons.Mapping;
using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Domain.Repositories;
using MISA.Salary.Infrastructure.Common;
using MISA.Salary.Infrastructure.Persistence.Repositories;
using System.Net.WebSockets;

namespace MISA.Salary.Application.UseCases.Implements;
public class SalaryCompositionService(
    ISalaryCompositionRepository salaryCompositionRepository, 
    ISalaryCompositionSystemRepository salaryCompositionSystemRepository) 
    : ISalaryCompostionService
{
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

    public async Task<Result> UpdateSalaryCompositionStatus(int salaryCompositionId, Status status)
    {
        var existingEntity = await salaryCompositionRepository.GetByIdAsync(salaryCompositionId);
        if (existingEntity == null)
        {
            return Result.Failure(404, new Error("NotFound", "Salary composition not found."));
        }

        var updated = await salaryCompositionRepository.UpdateSalaryCompositionStatus(salaryCompositionId, status);
        if (!updated)
        {
            return Result.Failure(400, new Error("UpdateFailed", "Failed to update status."));
        }

        return Result.Success(200);
    }

    public async Task<Result<string>> CreateSalaryCompositionFromSystemAsync(int salaryCompositionSystemId)
    {
        var salaryCompositionSystem = await salaryCompositionSystemRepository.GetByIdAsync(salaryCompositionSystemId);
        if (salaryCompositionSystem == null)
        {
            return Result<string>.Failure(404, new Error("NotFound", "Salary composition system not found."));
        }

        var salaryComposition = new SalaryComposition
        {
            SalaryCompositionName = salaryCompositionSystem.SalaryCompositionSystemName,
            SalaryCompositionCode = salaryCompositionSystem.SalaryCompositionSystemCode,
            CompositionType = salaryCompositionSystem.CompositionType,
            CompositionNature = salaryCompositionSystem.CompositionNature,
            Taxable = salaryCompositionSystem.Taxable,
            TaxDeduction = salaryCompositionSystem.TaxDeduction,
            Quota = salaryCompositionSystem.QuotaFormula,
            Formula = salaryCompositionSystem.Formula,
            ValueType = salaryCompositionSystem.ValueType,
            Description = salaryCompositionSystem.Description,
            Status = Status.Following,
            OptionShowPaycheck = salaryCompositionSystem.OptionShowPaycheck,
            IsNotAllowDelete = true,
        };

        await salaryCompositionRepository.AddAsync(salaryComposition);
        await salaryCompositionSystemRepository.RemoveFromSystemCompositionsAsync(salaryCompositionSystemId);

        return Result<string>.Success("Salary composition created from system successfully.", 201);
    }
}
