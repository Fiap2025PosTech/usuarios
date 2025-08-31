using Usuarios.Domain.Entities.Interfaces;

namespace Usuarios.Domain.Entities;

public class Identifier : IIdentifier
{
    public Guid Id { get; set; }
}