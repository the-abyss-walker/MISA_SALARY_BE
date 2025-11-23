using MISA.Salary.Application.Commons.Mapping;
using MISA.Salary.Application.Commons.Models.SalaryCompositionSystem;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Repositories;

namespace MISA.Salary.Application.UseCases.Implements;
public class SalaryCompositionSystemService : ISalaryCompositionSystemService
{
    private readonly ISalaryCompositionSystemRepository _salaryCompositionSystemRepository;

    public SalaryCompositionSystemService(
        ISalaryCompositionSystemRepository salaryCompositionRepository)
    {
        _salaryCompositionSystemRepository = salaryCompositionRepository;
    }

    public async Task<Result<PaginationResult<SalaryCompositionSystem>>> FilterSalaryCompositionSystemPaginationAsync(
        SalaryCompositionSystemParameter parameter)
    {
        var res = await _salaryCompositionSystemRepository.FilterPaginationAsync(parameter);
        return Result<PaginationResult<SalaryCompositionSystem>>.Success(res);
    }

    public async Task<Result<SalaryCompositionSystemResponse>> ExistCompositionCode(string salaryCompositionCode)
    {
        var isDuplicate = await _salaryCompositionSystemRepository.ExistCompositionCode(salaryCompositionCode);
        if (isDuplicate)
        {
            var salaryCompositionSystem = await _salaryCompositionSystemRepository.GetByCodeAsync(salaryCompositionCode);
            
            var res = SalaryCompositionSystemMapping.ToSalaryCompositionSystemResponse(salaryCompositionSystem);

            return Result<SalaryCompositionSystemResponse>.Success(res!);
        }
        return Result<SalaryCompositionSystemResponse>.Success(null!);
    }
}
