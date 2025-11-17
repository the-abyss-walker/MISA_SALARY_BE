using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Contract.Shared;

namespace MISA.Salary.Application.UseCases.Interfaces;
public interface ISalaryCompostionService
{
    Task<Result<PaginationResult<SalaryCompositionResponse>>> GetAllSalaryComposition();
    Task<Result<SalaryCompositionResponse>> CreateSalaryComposition(SalaryCompositionCreateRequest request);
}
