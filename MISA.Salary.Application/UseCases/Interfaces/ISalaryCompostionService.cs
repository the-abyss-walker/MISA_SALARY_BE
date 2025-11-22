using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Contract.Query;
using MISA.Salary.Contract.Shared;
using MISA.Salary.Domain.Entitites;
using MISA.Salary.Domain.Enums;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface ISalaryCompostionService
{
    Task<Result<IEnumerable<SalaryCompositionResponse>>> GetAllSalaryCompositionsAsync();
    Task<Result<PaginationResult<SalaryComposition>>> FilterSalaryCompositionPaginationAsync(
        SalaryCompositionParameter parameter);
    Task<Result<SalaryCompositionResponse>> GetSalaryCompositionById(int salaryCompositionId);
    Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request);
    Task<Result<SalaryCompositionResponse>> UpdateSalaryComposition(SalaryCompositionUpdateRequest request);
    Task<Result> DeleteSalaryComposition(int salaryCompositionId);
    Task<Result<DefaultCompositionCheckResponse>> CheckDefaultComposition(IEnumerable<int> salaryCompositionIds);
    Task<Result> BulkDeleteSalaryCompositions(IEnumerable<int> salaryCompositionIds);
    Task<Result> UpdateSalaryCompositionStatus(int salaryCompositionId, Status status);
    Task<Result> UpdateListSalaryCompositionStatus(IEnumerable<int> salaryCompositionIds, Status status);
    Task<Result<string>> CreateSalaryCompositionFromSystemAsync(IEnumerable<int> salaryCompositionSystemIds);
}
