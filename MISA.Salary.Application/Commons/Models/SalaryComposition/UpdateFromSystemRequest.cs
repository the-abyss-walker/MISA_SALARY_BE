namespace MISA.Salary.Application.Commons.Models.SalaryComposition;
public class UpdateFromSystemRequest
{
    public IEnumerable<int> SalaryCompositionSystemIds { get; set; } = [];
    public bool? IsAllowanceUpdate { get; set; }
}
