using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Contract.Shared;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface ISalaryCompostionService
{
    Task<Result<PaginationResult<SalaryCompositionResponse>>> GetAllSalaryComposition(int pageSize = 10, int pageIndex = 1);
    Task<Result<SalaryCompositionResponse>> GetSalaryCompositionById(int salaryCompositionId);
    Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request);
    Task<Result<SalaryCompositionResponse>> UpdateSalaryComposition(SalaryCompositionUpdateRequest request);
    Task<Result> DeleteSalaryComposition(int salaryCompositionId);
    Task<Result> BulkDeleteSalaryCompositions(IEnumerable<int> salaryCompositionIds);
}
