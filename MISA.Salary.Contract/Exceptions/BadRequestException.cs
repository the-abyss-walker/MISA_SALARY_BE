namespace MISA.Salary.Contract.Exceptions;
public class BadRequestException : ExceptionBase
{
    public BadRequestException(string message) : base(message)
    {
    }
}
