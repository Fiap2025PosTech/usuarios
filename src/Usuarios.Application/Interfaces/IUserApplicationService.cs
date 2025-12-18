using Usuarios.Application.Dto;

namespace Usuarios.Application.Interfaces;

public interface IUserApplicationService
{
    Task<User> Add(GuestUser model);
    Task<User> AddAdmin(GuestUser model);
}