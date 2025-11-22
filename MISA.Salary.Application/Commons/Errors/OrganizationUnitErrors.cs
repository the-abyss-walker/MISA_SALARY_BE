using MISA.Salary.Contract.Shared;

namespace MISA.Salary.Application.Commons.Errors;
public static class OrganizationUnitErrorMessages
{
    public const string OrganizationUnitNotFound = "Đơn vị tổ chức không tồn tại.";
}

public static class OrganizationUnitErrors
{
    public static Error OrganizationUnitNotFound =>
        new("OrganizationUnitNotFound", OrganizationUnitErrorMessages.OrganizationUnitNotFound);
}
