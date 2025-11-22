using MISA.Salary.Contract.Shared;

namespace MISA.Salary.Application.Commons.Errors;
public static class SalaryCompositionSystemErrorMessages
{
    public const string SalaryCompositionSystemNotfound = "Không tìm thấy thành phần lương.";
}

public static class SalaryCompositionSystemErrors
{
    public static Error SalaryCompositionSystemNotfound => 
        new("SalaryCompositionSystemNotfound", SalaryCompositionSystemErrorMessages.SalaryCompositionSystemNotfound);
}
