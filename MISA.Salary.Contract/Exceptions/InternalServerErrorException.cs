namespace MISA.Salary.Contract.Exceptions;
public class InternalServerErrorException : ExceptionBase
{
    public InternalServerErrorException(string message) : base(message)
    {
    }
}
