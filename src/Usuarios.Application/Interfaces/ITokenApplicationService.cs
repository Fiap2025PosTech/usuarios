using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Application.Dto;

namespace Usuarios.Application.Interfaces;

public interface ITokenApplicationService
{
    Task<string> GetToken(UserLogin userLogin);
    Task<string> GetTokenByAutorization(string? email);
}