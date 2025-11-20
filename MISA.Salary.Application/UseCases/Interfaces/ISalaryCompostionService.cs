using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;
using MISA.Salary.Infrastructure.Common;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface ISalaryCompostionService
{
    Task<Result<PaginationResult<SalaryComposition>>> FilterSalaryCompositionPaginationAsync(
        SalaryCompositionParameter parameter);
    Task<Result<SalaryCompositionResponse>> GetSalaryCompositionById(int salaryCompositionId);
    Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request);
    Task<Result<SalaryCompositionResponse>> UpdateSalaryComposition(SalaryCompositionUpdateRequest request);
    Task<Result> DeleteSalaryComposition(int salaryCompositionId);
    Task<Result> BulkDeleteSalaryCompositions(IEnumerable<int> salaryCompositionIds);
    Task<Result> UpdateSalaryCompositionStatus(int salaryCompositionId, Status status);
}
