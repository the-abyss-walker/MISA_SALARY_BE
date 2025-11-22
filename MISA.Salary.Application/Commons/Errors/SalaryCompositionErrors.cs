using MISA.Salary.Contract.Shared;

namespace MISA.Salary.Application.Commons.Errors;

public static class SalaryCompositionErrorMessages
{
    public const string SalaryCompositionNotFound = "Không tìm thấy thành phần lương.";
    public const string SalaryCompositionCodeExists = "Mã thành phần lương đã tồn tại.";
    public const string SalaryCompositionDefault =
        "Đây là thành phần lương mặc định của hệ thống nên không thể xóa. Vui lòng kiểm tra lại.";
    public const string DeleteSalaryCompositionFailed =
        "Xóa thành phần lương thất bại. Vui lòng thử lại.";
    public const string SalaryCompositionUpdateStatusFailed =
        "Cập nhật trạng thái thành phần lương thất bại. Vui lòng thử lại.";
}
public static class SalaryCompositionErrors
{
    public static Error SalaryCompositionNotFound =>
        new("SalaryCompositionNotFound", SalaryCompositionErrorMessages.SalaryCompositionNotFound);

    public static Error SalaryCompositionCodeExists =>
        new("SalaryCompositionCodeExists", SalaryCompositionErrorMessages.SalaryCompositionCodeExists);

    public static Error SalaryCompositionDefault =>
        new("SalaryCompositionUsedInSystem", SalaryCompositionErrorMessages.SalaryCompositionDefault);

    public static Error DeleteSalaryCompositionFailed =>
        new("DeleteSalaryCompositionFailed", SalaryCompositionErrorMessages.DeleteSalaryCompositionFailed);

    public static Error SalaryCompositionUpdateStatusFailed =>
        new("UpdateSalaryCompositionStatusFailed", SalaryCompositionErrorMessages.SalaryCompositionUpdateStatusFailed);
}
