namespace MISA.Salary.Domain.Abstract;
public interface IEntity<T>
{
    public T Id { get; set; }
}
