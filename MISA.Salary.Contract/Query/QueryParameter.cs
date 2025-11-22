namespace MISA.Salary.Contract.Query;
public abstract class QueryParameter
{
    public int PageSize { get; set; } = 15;
    public int PageIndex { get; set; } = 1;
}
