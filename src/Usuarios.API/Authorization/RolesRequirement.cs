using Microsoft.AspNetCore.Authorization;
using Usuarios.Domain.Enums;

namespace Usuarios.API.Authorization;

  public class RolesRequirement(AccessLevel level) : IAuthorizationRequirement
  {
      public AccessLevel AccessLevel { get; } = level;
  }