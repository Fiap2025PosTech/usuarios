
using Usuarios.Domain.Entities;
using Usuarios.Domain.Interfaces;
using Usuarios.Domain.Interfaces.Infraestructure;

namespace Usuarios.Domain.Services;

public class UserService(IUserRepository userRepository, UserData userData) : BaseService<User>(userRepository, userData), IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User> GetById(Guid id)
    {
        return await _userRepository.GetById(id, false, false);
    }

    public override async Task<User> Add(User entity)
    {
        var user = await _userRepository.GetByEmail(entity.Email);

        if (user != null)
            throw new ArgumentException("O usuário já existe.");

        entity.SetDefaultUser();
        entity.PrepareToInsert(_userData.Id);

        return await base.Add(entity);
    }

    public async Task<User> GetByEmail(string? email)
    {
        return await _userRepository.GetByEmail(email);
    }
}