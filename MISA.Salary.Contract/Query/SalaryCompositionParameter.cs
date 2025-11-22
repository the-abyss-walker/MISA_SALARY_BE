using System.ComponentModel.DataAnnotations;

namespace MISA.Salary.Contract.Query;
public class SalaryCompositionParameter : QueryParameter
{
    public string? Query { get; set; }
    public int? Status { get; set; }
}
