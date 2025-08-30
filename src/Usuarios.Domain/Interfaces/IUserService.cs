namespace Usuarios.Domain.Interfaces;

public interface IUserService
{
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string? email);
}