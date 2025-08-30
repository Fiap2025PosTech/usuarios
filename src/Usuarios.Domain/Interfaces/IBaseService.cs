namespace Usuarios.Domain.Interfaces;

public interface IBaseService<T>  : IRepository<T> where T : class, IBaseEntity
{
    Task Remove(T entity);
}