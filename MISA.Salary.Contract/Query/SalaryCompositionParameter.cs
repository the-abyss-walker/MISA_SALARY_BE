namespace MISA.Salary.Contract.Query;
public class SalaryCompositionParameter : QueryParameter
{
    public string? Query { get; set; }
    public int? Status { get; set; }
    public List<int>? OrganizationUnitIds { get; set; }
}
