using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Contract.Shared;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface ISalaryCompostionService
{
    Task<Result<PaginationResult<SalaryCompositionResponse>>> GetAllSalaryComposition(int pageSize = 10, int pageIndex = 1);
    Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request);
}
