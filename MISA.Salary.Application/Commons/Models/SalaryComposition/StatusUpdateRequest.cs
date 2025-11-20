using FluentValidation;
using MISA.Salary.Domain.Enums;

namespace MISA.Salary.Application.Commons.Models.SalaryComposition;
public class StatusUpdateRequest
{
    public Status Status { get; set; }
}

public class StatusUpdateRequestValidator : AbstractValidator<StatusUpdateRequest>
{
    public StatusUpdateRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Trạng thái không hợp lệ.");
    }
}
