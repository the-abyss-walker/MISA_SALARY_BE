using MISA.Salary.Application.Commons.Mapping;
using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Infrastructure.Persistence.Repositories;

namespace MISA.Salary.Application.UseCases.Implements;
public class SalaryCompositionService(ISalaryCompositionRepository salaryCompositionRepository) 
    : ISalaryCompostionService
{
    public async Task<Result<PaginationResult<SalaryCompositionResponse>>> GetAllSalaryComposition()
    {
        var salaryCompositions = await salaryCompositionRepository.GetAllAsync();

        var res = salaryCompositions.Select(SalaryCompositionMapping.ToSalaryCompositionResponse);

        return PaginationResult<SalaryCompositionResponse>.Create(5, 1, 50, [.. res]);
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
