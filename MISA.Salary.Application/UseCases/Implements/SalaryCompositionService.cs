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

    public async Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request)
    {
        // Map request to entity
        var salaryComposition = SalaryCompositionMapping.ToSalaryCompositionEntity(request);
        // Save to database
        await salaryCompositionRepository.AddAsync(salaryComposition);

        var res = SalaryCompositionMapping.ToSalaryCompositionResponse(salaryComposition);
        return Result<SalaryCompositionResponse>.Success(res, 201);
    }
}
