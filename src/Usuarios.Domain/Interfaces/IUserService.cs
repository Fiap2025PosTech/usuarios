using Usuarios.Domain.Entities;

namespace Usuarios.Domain.Interfaces;

public interface IUserService : IBaseService<User>
{
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string? email);
}